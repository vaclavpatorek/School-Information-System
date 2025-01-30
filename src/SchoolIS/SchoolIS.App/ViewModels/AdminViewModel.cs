using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Enums;
using zoft.MauiExtensions.Core.Extensions;

namespace SchoolIS.App.ViewModels;

public partial class AdminViewModel(
  IMessengerService messengerService,
  INavigationService navigationService,
  IAuthenticationService authenticationService,
  ISubjectFacade subjectFacade,
  IHasSubjectFacade hasSubjectFacade,
  IUserFacade userFacade,
  IUserModelMapper userModelMapper,
  ISubjectModelMapper subjectModelMapper
) : ViewModelBase(messengerService, navigationService, authenticationService) {
  // Edited user / subject
  [ObservableProperty] private UserDetailModel _newUser = UserDetailModel.Empty;
  [ObservableProperty] private SubjectDetailModel _newSubject = SubjectDetailModel.Empty;

  [ObservableProperty] private bool _isEditingUser;
  [ObservableProperty] private bool _isEditingSubject;

  // Error messages
  [ObservableProperty] private string _userErrorMessage = "";
  [ObservableProperty] private string _subjectErrorMessage = "";

  // Filters
  [ObservableProperty] private UserType? _activeUserTypeFilter;
  [ObservableProperty] private ObservableCollection<UserListModel> _filteredUsers = [];
  [ObservableProperty] private ObservableCollection<SubjectListModel> _filteredSubjects = [];
  [ObservableProperty] private ObservableCollection<SubjectListModel> _filteredUserSubjects = [];

  [ObservableProperty] private ObservableCollection<Guid> _userSubjects = [];
  [ObservableProperty] private ObservableCollection<UserListModel> _teachers = [];


  public ObservableCollection<UserListModel> Users { get; set; } = [];
  public ObservableCollection<SubjectListModel> Subjects { get; set; } = [];
  [ObservableProperty] private ObservableCollection<HasSubjectListModel> _subjectTeachers = [];

  private readonly Dictionary<string, Func<UserListModel, bool>?> _userFilters = new() {
    { nameof(UserType), null },
    { nameof(UserListModel.FullName), null }
  };

  private readonly Dictionary<string, Func<SubjectListModel, bool>?> _subjectFilters = new() {
    { nameof(SubjectListModel.Shortcut), null },
    { nameof(SubjectListModel.Name), null },
  };

  protected override async Task LoadDataAsync() {
    await base.LoadDataAsync();
    await Task.WhenAll(LoadSubjectsAsync(), LoadUsersAsync());
  }

  private async Task LoadUsersAsync() {
    Users = (await userFacade.GetAsync(orderBy: nameof(UserListModel.Login), desc: false))
      .ToObservableCollection();
    FilteredUsers = new ObservableCollection<UserListModel>(Users);
  }

  private async Task LoadSubjectsAsync() {
    Subjects =
      (await subjectFacade.GetAsync(orderBy: nameof(SubjectListModel.Shortcut), desc: false))
      .ToObservableCollection();
    FilteredSubjects = new ObservableCollection<SubjectListModel>(Subjects);
    FilteredUserSubjects = new ObservableCollection<SubjectListModel>(Subjects);
  }

  [RelayCommand]
  private void SelectSubject(SubjectListModel subject) {
    if (!UserSubjects.Remove(subject.Id)) {
      UserSubjects.Add(subject.Id);
    }

    UserSubjects = new ObservableCollection<Guid>(UserSubjects);
  }

  [RelayCommand]
  private async Task DeleteUserAsync(UserListModel user) {
    try {
      await userFacade.DeleteAsync(user.Id);
      await RemoveUser(user);
    } catch (InvalidOperationException e) {
      UserErrorMessage = e.Message;
    }
  }

  [RelayCommand]
  private async Task DeleteSubjectAsync(SubjectListModel subject) {
    try {
      await subjectFacade.DeleteAsync(subject.Id);
      RemoveSubject(subject);
    } catch (InvalidOperationException e) {
      SubjectErrorMessage = e.Message;
    }
  }

  private void RemoveSubject(SubjectListModel subject) {
    Subjects.Remove(subject);
    FilteredSubjects.Remove(subject);
    FilteredUserSubjects.Remove(subject);
  }

  private async Task RemoveUser(UserListModel user) {
    Users.Remove(user);
    FilteredUsers.Remove(user);
    await EditSubjectAsync(NewSubject.Id);
  }

  [RelayCommand]
  private async Task EditUserAsync(Guid userId) {
    try {
      UserDetailModel? userDetailModel = await userFacade.GetAsync(userId);

      if (userDetailModel == null) {
        UserErrorMessage = "User not found";
        return;
      }

      // Do not show password
      userDetailModel.Password = null;

      NewUser = userDetailModel;
      UserSubjects = new ObservableCollection<Guid>(NewUser.Subjects.Select(e => e.SubjectId));
      IsEditingUser = true;
    } catch (InvalidOperationException e) {
      UserErrorMessage = e.Message;
    }
  }

  [RelayCommand]
  private async Task EditSubjectAsync(Guid subjectId) {
    try {
      SubjectDetailModel? subjectDetailModel = await subjectFacade.GetAsync(subjectId);

      if (subjectDetailModel == null) {
        SubjectErrorMessage = "Subject not found";
        return;
      }

      NewSubject = subjectDetailModel;
      IsEditingSubject = true;
      SubjectTeachers = new ObservableCollection<HasSubjectListModel>(NewSubject.Teachers);
    } catch (InvalidOperationException e) {
      SubjectErrorMessage = e.Message;
    }
  }

  [RelayCommand]
  private async Task AddUserAsync() {
    try {
      HashPassword();

      UserDetailModel addedUser = await UpdateUser();
      await UpdateUserSubjects(addedUser);

      ApplyUserFilters();
      CancelUserEditingCommand.Execute(null);
    } catch (InvalidOperationException e) {
      UserErrorMessage = e.Message;
    }
  }


  [RelayCommand]
  private async Task AddSubjectAsync() {
    try {
      SubjectDetailModel addedSubject = await UpdateSubject();
      await UpdateSubjectTeachers(addedSubject.Id);

      ApplySubjectFilters();
      CancelSubjectEditingCommand.Execute(null);
    } catch (InvalidOperationException e) {
      SubjectErrorMessage = e.Message;
    }
  }

  [RelayCommand]
  private void CancelUserEditing() {
    IsEditingUser = false;
    NewUser = UserDetailModel.Empty;
    UserSubjects = [];
  }

  [RelayCommand]
  private void CancelSubjectEditing() {
    IsEditingSubject = false;
    NewSubject = SubjectDetailModel.Empty;
    SubjectTeachers = [];
  }

  [RelayCommand]
  private void RemoveSubjectTeacher(HasSubjectListModel teacher) {
    SubjectTeachers.Remove(teacher);
  }

  [RelayCommand]
  private void SearchSubject(string text) {
    string searchedShortcut = text.ToLower();

    FilteredUserSubjects =
      new ObservableCollection<SubjectListModel>(Subjects.Where(subject =>
        subject.Shortcut.Contains(searchedShortcut, StringComparison.CurrentCultureIgnoreCase)));
  }

  [RelayCommand]
  private void SelectUserType(UserType userType) {
    NewUser.Type = userType;
  }

  [RelayCommand]
  private void FilterByUserType(UserType userType) {
    if (userType == ActiveUserTypeFilter) {
      // Remove user type filter
      _userFilters.Remove(nameof(UserType));
      ActiveUserTypeFilter = null;
    } else {
      ActiveUserTypeFilter = userType;
      _userFilters[nameof(UserType)] = user => user.Type == userType;
    }

    ApplyUserFilters();
  }

  [RelayCommand]
  private void SearchByName(string text) {
    string searchedName = text.ToLower();

    _userFilters[nameof(UserListModel.FullName)] = user => {
      var concatenatedLowerLastName = user.FullName.ToLower();
      return concatenatedLowerLastName.Contains(searchedName);
    };

    ApplyUserFilters();
  }

  [RelayCommand]
  private void SearchBySubjectShortcut(string text) {
    string searchShortcut = text.ToLower();

    _subjectFilters[nameof(SubjectListModel.Shortcut)] =
      subject =>
        subject.Shortcut.Contains(searchShortcut, StringComparison.CurrentCultureIgnoreCase);

    ApplySubjectFilters();
  }

  [RelayCommand]
  private void SearchBySubjectName(string text) {
    string searchName = text.ToLower();

    _subjectFilters[nameof(SubjectListModel.Name)] =
      subject => subject.Name.Contains(searchName, StringComparison.CurrentCultureIgnoreCase);

    ApplySubjectFilters();
  }

  private void ApplyUserFilters() {
    FilteredUsers = new ObservableCollection<UserListModel>(
      Users.Where(user =>
        _userFilters.All(filter => filter.Value == null || filter.Value(user)
        )
      ).OrderBy(user => user.Login)
    );
  }

  private void ApplySubjectFilters() {
    FilteredSubjects = new ObservableCollection<SubjectListModel>(
      Subjects.Where(subject =>
        _subjectFilters.All(filter => filter.Value == null || filter.Value(subject)
        )
      ).OrderBy(subject => subject.Shortcut)
    );
  }

  public void FilterTeacherList(string text) {
    Teachers.Clear();

    string filter = text.TrimEnd();

    Teachers.AddRange(Users.Where(user =>
      user.Type == UserType.Teacher &&
      user.FullName.Contains(filter, StringComparison.CurrentCultureIgnoreCase) &&
      SubjectTeachers.All(teacher => teacher.UserId != user.Id))
    );
  }

  public bool TryAddTeacherToSubject(string filter) {
    string fullName = filter.TrimEnd();
    UserListModel? teacher = Teachers.FirstOrDefault(teacher => teacher.FullName == fullName);

    if (teacher == null) return false;

    SubjectTeachers.Add(HasSubjectListModel.Empty with {
      UserId = teacher.Id,
      SubjectId = NewSubject.Id,
      UserFirstName = teacher.FirstName,
      UserLastName = teacher.LastName,
      UserType = teacher.Type,
    });

    Teachers.Clear();
    return true;
  }


  private async Task<UserDetailModel> UpdateUser() {
    // Save or update user
    UserDetailModel addedUser = await userFacade.SaveAsync(NewUser with {
      Subjects = default!,
    });

    // Remove old version of the updated user
    var userToRemove = Users.FirstOrDefault(user => user.Id == addedUser.Id);

    // If the user is found, replace it
    if (userToRemove != null) {
      userToRemove.Login = addedUser.Login;
      userToRemove.FirstName = addedUser.FirstName;
      userToRemove.LastName = addedUser.LastName;
      userToRemove.Type = addedUser.Type;
    } else {
      // Add user to Users list
      UserListModel user = userModelMapper.MapToListModel(userModelMapper.MapToEntity(addedUser));
      Users.Add(user);
    }

    return addedUser;
  }

  private async Task UpdateUserSubjects(UserDetailModel addedUser) {
    NewUser.Subjects.Select(e => e.SubjectId).GetEnumerableDifference(UserSubjects,
      out IEnumerable<Guid> added, out IEnumerable<Guid> removed);

    // Save all changes in user subjects
    IEnumerable<Guid> addedList = added.ToList();
    IEnumerable<Guid> removedList = removed.ToList();

    await Task.WhenAll([
      ..addedList.Select(subjectId => SaveUserSubject(addedUser.Id, subjectId)),
      ..removedList.Select(DeleteUserSubject)
    ]);

    // Refresh subject data if 
    if (addedList.Any(id => id == NewSubject.Id) || removedList.Any(id => id == NewSubject.Id)) {
      await EditSubjectCommand.ExecuteAsync(NewSubject.Id);
    }
  }


  private async Task<SubjectDetailModel> UpdateSubject() {
    // Save user
    SubjectDetailModel addedSubject = await subjectFacade.SaveAsync(NewSubject with {
      Students = default!,
      Teachers = default!,
    });

    // Remove old version of the updated user
    var subjectToRemove = Subjects.FirstOrDefault(subject => subject.Id == addedSubject.Id);

    // If the user is found, replace it
    if (subjectToRemove != null) {
      subjectToRemove.Name = addedSubject.Name;
      subjectToRemove.Shortcut = addedSubject.Shortcut;
    } else {
      // Add user to Users list
      SubjectListModel subject =
        subjectModelMapper.MapToListModel(subjectModelMapper.MapToEntity(addedSubject));
      Subjects.Add(subject);
      FilteredUserSubjects.Add(subject);
    }

    return addedSubject;
  }

  private async Task UpdateSubjectTeachers(Guid addedSubjectId) {
    NewSubject.Teachers.GetEnumerableDifference(SubjectTeachers,
      out IEnumerable<HasSubjectListModel> added, out IEnumerable<HasSubjectListModel> removed);

    // Save all changes in user subjects
    IEnumerable<HasSubjectListModel> addedList = added.ToList();
    IEnumerable<HasSubjectListModel> removedList = removed.ToList();

    await Task.WhenAll([
      ..addedList.Select(model => SaveSubjectTeacher(model.UserId, addedSubjectId)),
      ..removedList.Select(model => DeleteSubjectTeacher(model.UserId))
    ]);

    // Refresh subject data if 
    if (addedList.Any(model => model.UserId == NewUser.Id) ||
        removedList.Any(model => model.UserId == NewUser.Id)) {
      await EditUserCommand.ExecuteAsync(NewUser.Id);
    }
  }


  private async Task SaveUserSubject(Guid userId, Guid subjectId) {
    HasSubjectDetailModel hasSubjectDetailModel = HasSubjectDetailModel.Empty with {
      Id = Guid.NewGuid(),
      UserId = userId,
      SubjectId = subjectId,
    };

    await hasSubjectFacade.SaveAsync(hasSubjectDetailModel, userId);
  }

  private async Task DeleteUserSubject(Guid subjectId) {
    HasSubjectListModel toDelete = NewUser.Subjects.First(e => e.SubjectId == subjectId);
    await hasSubjectFacade.DeleteAsync(toDelete.Id);
  }

  private async Task DeleteSubjectTeacher(Guid teacherId) {
    HasSubjectListModel toDelete = NewSubject.Teachers.First(e => e.UserId == teacherId);
    await hasSubjectFacade.DeleteAsync(toDelete.Id);
  }

  private async Task SaveSubjectTeacher(Guid teacherId, Guid subjectId) {
    HasSubjectDetailModel model = HasSubjectDetailModel.Empty with {
      Id = Guid.NewGuid(),
      SubjectId = subjectId,
      UserId = teacherId,
    };

    await hasSubjectFacade.SaveAsync(model, teacherId);
  }

  private void HashPassword() {
    if (!String.IsNullOrEmpty(NewUser.Password)) {
      NewUser.Password = NewUser.Password.Hash();
    }
  }
}