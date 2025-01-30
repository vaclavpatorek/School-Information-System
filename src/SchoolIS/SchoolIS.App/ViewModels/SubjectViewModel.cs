using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Enums;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.Messaging;
using SchoolIS.App.Messages;
using SchoolIS.BL.Mappers.Interfaces;

namespace SchoolIS.App.ViewModels;

public partial class SubjectViewModel(
  IMessengerService messengerService,
  INavigationService navigation,
  IAuthenticationService authenticationService,
  IRoomFacade roomFacade,
  IUserFacade userFacade,
  IActivityFacade activityFacade,
  IHasSubjectFacade hasSubjectFacade,
  ISubjectFacade subjectFacade,
  IActivityModelMapper activityModelMapper)
  : ViewModelBase(messengerService, navigation, authenticationService), IQueryAttributable,
    IRecipient<EvaluationsUpdatedMessage> {
  public Guid SubjectId { get; set; }

  [ObservableProperty] private SubjectDetailModel _subject = null!;
  [ObservableProperty] private ObservableCollection<ActivityListModel> _activities = null!;

  [ObservableProperty] private ObservableCollection<HasSubjectDetailModel> _students = null!;

  // Contains all students that do not have this subject.
  [ObservableProperty] private ObservableCollection<UserListModel> _otherStudents = null!;

  // New activity or the one being edited.
  [ObservableProperty] private ActivityDetailModel _newActivity = null!;
  [ObservableProperty] private bool _editingActivity;

  // Helper date getter and setter that automatically changes date in both StartsFrom and EndsAt.
  private DateTime _newDate = DateTime.Today.Date;

  public DateTime NewDate {
    get => _newDate;
    set {
      if (_newDate != value) {
        _newDate = value;
        NewActivity.StartsFrom = value.Date + NewActivity.StartsFrom.TimeOfDay;
        NewActivity.EndsAt = value.Date + NewActivity.EndsAt.TimeOfDay;
        OnPropertyChanged();
      }
    }
  }

  // Room selection
  [ObservableProperty] private ObservableCollection<RoomListModel> _rooms = null!;

  // Set room id in NewActivity when the selected room changes.
  private RoomListModel _selectedRoom = null!;

  public RoomListModel SelectedRoom {
    get => _selectedRoom;
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    set {
      if (_selectedRoom != value && value != null) {
        _selectedRoom = value;
        if (NewActivity != null) {
          NewActivity.RoomId = value.Id;
          NewActivity.RoomName = value.Name;
        }

        OnPropertyChanged();
      }
    }
  }

  // Stores what ActivityType filters are active.
  [ObservableProperty] private BitVector32 _activityTypeFilter = new(0);

  // Stores if the activities should be sorted in AscendingOrder.
  [ObservableProperty] private bool _dateFilterAsc;

  // Error message to display above save/add button.
  [ObservableProperty] private string _errorMessage = string.Empty;

  // Stores string that identify what the problem in new activity form is.
  // For example: date (for problem with date field) or startsFrom
  [ObservableProperty] private ObservableCollection<string> _problemsWith = [];

  // Teacher should be the currently logged-in user.
  private Guid Teacher {
    get {
      var user = AuthenticationService.GetLoggedInUser();
      if (user.Type != UserType.Teacher)
        throw new InvalidOperationException(
          "Subjects page for user type 'Student' is not supported.");
      return user.Id;
    }
  }

  // Set error state. This will have the impact of displaying error message and marking error fields.
  private void SetError(string with, string message) {
    ErrorMessage = message;
    ProblemsWith.Add(with);
    ProblemsWith = new ObservableCollection<string>(ProblemsWith);
  }

  // Clear error state.
  public void ClearErrors() {
    ErrorMessage = string.Empty;
    ProblemsWith = [];
  }

  [RelayCommand]
  private void EditActivity(ActivityListModel activity) {
    NewActivity.Id = activity.Id;
    NewActivity.ActivityType = activity.ActivityType;
    NewActivity.StartsFrom = activity.StartsFrom;
    NewActivity.EndsAt = activity.EndsAt;
    SelectedRoom = new RoomListModel {
      Id = activity.RoomId,
      Name = activity.RoomName
    };
    EditingActivity = true;
    NewDate = activity.StartsFrom;
    OnPropertyChanged(nameof(EditingActivity));
  }

  // Cancel editing of current activity
  [RelayCommand]
  private void CancelActivity() {
    EditingActivity = false;
    NewActivity.Id = Guid.Empty;
  }

  private bool CheckValidity() {
    if (NewActivity.EndsAt < NewActivity.StartsFrom) {
      SetError("EndsAt", "Activity end time cannot be before start time.");
      return false;
    }

    if (NewActivity.StartsFrom < DateTime.Now && !EditingActivity) {
      string problemWith = "";
      if (NewActivity.StartsFrom.Date < DateTime.Now.Date)
        problemWith = "Date";
      else if (NewActivity.StartsFrom < DateTime.Now)
        problemWith = "StartsFrom";
      SetError(problemWith, "Cannot define activity that starts in the past.");
      return false;
    }

    return true;
  }

  [RelayCommand]
  private async Task ConfirmActivityAsync() {
    ClearErrors();

    if (!CheckValidity())
      return;
    if (EditingActivity)
      await SaveActivityAsync();
    else
      await CreateActivity();
    EditingActivity = false;
    NewActivity.Id = Guid.Empty;
  }

  // Insert the newly created activity to DB.
  private async Task CreateActivity() {
    try {
      NewActivity.Id = Guid.Empty;
      var insertedActivity = await activityFacade.SaveAsync(NewActivity);
      Activities.Add(activityModelMapper.MapToListModel(insertedActivity));
    } catch (InvalidOperationException ex) {
      Trace.WriteLine("Exception caught! Error: " + ex);
      ErrorMessage = "Activity creation failed.";
    }
  }

  // Save currently edited activity to DB.
  private async Task SaveActivityAsync() {
    if (NewActivity.Id == Guid.Empty)
      return;

    try {
      var updatedActivity = activityModelMapper.MapToListModel(
        await activityFacade.SaveAsync(NewActivity));
      Activities.Remove(Activities.Single(a => a.Id == updatedActivity.Id));
      Activities.Add(updatedActivity);
    } catch (InvalidOperationException ex) {
      Trace.WriteLine("Exception caught! Error: " + ex);
      ErrorMessage = "Activity update failed.";
    }
  }

  [RelayCommand]
  private void ToggleActivityTypeFilter(ActivityType type) {
    var bits = ActivityTypeFilter;
    bits[1 << (int)type] ^= true;
    ActivityTypeFilter = bits;
  }

  [RelayCommand]
  private void ToggleDateFilter() {
    DateFilterAsc = !DateFilterAsc;
  }

  [RelayCommand]
  private void SelectActivityType(ActivityType type) {
    NewActivity.ActivityType = type;
  }

  [RelayCommand]
  private async Task DeleteActivityAsync(ActivityListModel activity) {
    try {
      Activities.Remove(Activities.Single(a => a.Id == activity.Id));
      await activityFacade.DeleteAsync(activity.Id);
      //ApplyFilters();
    } catch (InvalidOperationException ex) {
      Trace.WriteLine("Exception caught! Error: " + ex);
      ErrorMessage = "Activity update failed.";
    }
  }

  [RelayCommand]
  private async Task RemoveStudentAsync(HasSubjectDetailModel student) {
    try {
      await hasSubjectFacade.DeleteAsync(student.Id);
      Students.Remove(student);

      Students = new ObservableCollection<HasSubjectDetailModel>(Students);

      // Add student to other students so that it is suggested
      OtherStudents.Add(new() {
        FirstName = student.UserFirstName,
        LastName = student.UserLastName,
        Login = student.UserLogin,
        Type = UserType.Student,
        Id = student.UserId
      });
    } catch (InvalidOperationException e) {
      Trace.WriteLine($"Student removal failed. Exception: {e}");
      SetError("", "Student removal failed.");
    }
  }

  // Add student to subject
  [RelayCommand]
  private async Task AddStudentAsync(UserListModel student) {
    try {
      // Add student to subject
      HasSubjectDetailModel hasSubject = HasSubjectDetailModel.Empty with {
        SubjectId = Subject.Id,
        UserId = student.Id,
        UserType = UserType.Student,
      };
      var newStudent = await hasSubjectFacade.SaveAsync(hasSubject, student.Id);
      Students.Add(newStudent);

      Students = new ObservableCollection<HasSubjectDetailModel>(Students);

      // Remove student from other students, so that it isn't suggested.
      OtherStudents.Remove(OtherStudents.Single(s => s.Id == student.Id));
    } catch (InvalidOperationException e) {
      Trace.WriteLine($"Could not add student to subject. Exception: {e}");
      SetError("", "Could not add student to subject.");
    }
  }

  // Get all data from database.
  public async void ApplyQueryAttributes(IDictionary<string, object> query) {
    if (Guid.TryParse(query[nameof(SubjectDetailModel.Id)].ToString(), out Guid parsedGuid)) {
      SubjectId = parsedGuid;
      await LoadDataAsync();
    } else {
      throw new InvalidDataException("Invalid uri parameter for subject page");
    }
  }

  protected override async Task LoadDataAsync() {
    if (SubjectId == Guid.Empty) return;

    await base.LoadDataAsync();
    var subjectTask = subjectFacade.GetAsync(SubjectId);
    var roomsTask = roomFacade.GetAsync();
    var teacherTask = userFacade.GetAsync(Teacher);
    var activitiesTask = activityFacade.FilterActivitiesByDateAndSubject(
      SubjectId, DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1));
    var allStudentsTask = userFacade.GetAsync("FirstName");

    await Task.WhenAll(subjectTask, roomsTask, teacherTask, activitiesTask, allStudentsTask);

    var subject = subjectTask.Result;
    var rooms = roomsTask.Result.ToObservableCollection();
    var teacher = teacherTask.Result;
    var activities = activitiesTask.Result;
    var allStudents = allStudentsTask.Result;

    if (subject == null || rooms == null || teacher == null || activities == null ||
        allStudents == null) {
      // TODO: handle failed database load
      throw new NotImplementedException(
        "Subject or room or teacher or activities or allStudents returned null.");
    }

    // Get all detail models for students in subject. This will give us total points for each student.
    List<HasSubjectDetailModel> students = [];
    foreach (var h in subject!.Students) {
      var details = await hasSubjectFacade.GetAsync(h.Id);
      students.Add(details!);
    }

    Students = students.ToObservableCollection();
    Students.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Students));

    // Check rooms
    if (!rooms.Any()) {
      Trace.WriteLine("Warning: No rooms found.");
    }

    // Initialize ViewModel's fields with received data.
    Subject = subject;
    Activities = activities.ToObservableCollection();
    Activities.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Activities));
    Rooms = rooms;
    SelectedRoom = Rooms.ElementAtOrDefault(0) ?? new RoomListModel();
    OtherStudents = allStudents
      .Where(s => Students
        .FirstOrDefault(h => h.UserId == s.Id) == null && s.Type == UserType.Student)
      .ToObservableCollection();

    // Initialize initial empty new activity.
    NewActivity = new() {
      CreatorId = teacher.Id,
      CreatorFirstName = teacher.FirstName,
      CreatorLastName = teacher.LastName,
      RoomId = SelectedRoom.Id,
      RoomName = SelectedRoom.Name,
      ActivityType = ActivityType.Lecture,
      StartsFrom = DateTime.Today.AddHours(7),
      EndsAt = DateTime.Today.AddHours(8).AddMinutes(50),
      SubjectId = SubjectId,
      Evaluations = [],
    };

    ClearErrors();
    EditingActivity = false;
  }

  public async void Receive(EvaluationsUpdatedMessage message) {
    if (message.SubjectId != Subject.Id)
      return;

    await LoadDataAsync();
  }
}