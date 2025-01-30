using SchoolIS.App.ViewModels;
using SchoolIS.BL.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using zoft.MauiExtensions.Controls;

namespace SchoolIS.App.Views;

public partial class AdminPage : ContentPageBase {
  private readonly AdminViewModel _adminViewModel;
  public ObservableCollection<string> InvalidUserFields { get; set; } = [];
  public ObservableCollection<string> InvalidSubjectFields { get; set; } = [];

  public AdminPage(AdminViewModel adminViewModel) : base(adminViewModel) {
    InitializeComponent();
    this._adminViewModel = adminViewModel;
  }

  [RelayCommand]
  private async Task ValidateDataAndAddUser() {
    UserErrorMessage.Text = "";
    if (ValidateUserFieldValues()) {
      await _adminViewModel.AddUserCommand.ExecuteAsync(null);
    }
  }

  [RelayCommand]
  private async Task ValidateDataAndAddSubject() {
    SubjectErrorMessage.Text = "";
    if (ValidateSubjectFieldValues()) {
      await _adminViewModel.AddSubjectCommand.ExecuteAsync(null);
    }
  }

  [RelayCommand]
  private void CancelUserEditing() {
    _adminViewModel.CancelUserEditingCommand.Execute(null);
    ClearUserErrors();
  }

  [RelayCommand]
  private void CancelSubjectEditing() {
    _adminViewModel.CancelSubjectEditingCommand.Execute(null);
    ClearSubjectErrors();
  }

  [RelayCommand]
  private async Task EditUser(Guid userId) {
    await _adminViewModel.EditUserCommand.ExecuteAsync(userId);
    ClearUserErrors();
  }

  [RelayCommand]
  private async Task EditSubject(Guid subjectId) {
    await _adminViewModel.EditSubjectCommand.ExecuteAsync(subjectId);
    ClearSubjectErrors();
  }

  private void ClearUserErrors() {
    InvalidUserFields.Clear();
    UserErrorMessage.Text = "";
    OnPropertyChanged(nameof(InvalidUserFields));
  }

  private void ClearSubjectErrors() {
    InvalidSubjectFields.Clear();
    SubjectErrorMessage.Text = "";
    OnPropertyChanged(nameof(InvalidSubjectFields));
  }


  private void AutoCompleteEntry_TextChanged(object sender,
    AutoCompleteEntryTextChangedEventArgs e) {
    // Filter only when the user is typing
    if (e.Reason == AutoCompleteEntryTextChangeReason.UserInput) {
      //Filter the ItemsSource, based on text
      string filter = (sender as AutoCompleteEntry)!.Text;
      _adminViewModel.FilterTeacherList(filter);


      if (filter.EndsWith(' ') && _adminViewModel.TryAddTeacherToSubject(filter)) {
        TeacherEntry.Text = "";
      }
    }
  }

  private bool ValidateUserFieldValues() {
    bool valid = true;
    InvalidUserFields.Clear();

    // Login
    if (!String.IsNullOrEmpty(Login.Text) && Login.Text.Length != 8) {
      valid = false;
      UserErrorMessage.Text = "Invalid login";
      InvalidUserFields.Add(nameof(UserDetailModel.Login));
    }

    // First name
    if (String.IsNullOrEmpty(FirstName.Text)) {
      valid = false;
      UserErrorMessage.Text = "First name is required";
      InvalidUserFields.Add(nameof(UserDetailModel.FirstName));
    }

    // Last name
    if (String.IsNullOrEmpty(LastName.Text)) {
      valid = false;
      UserErrorMessage.Text = "Last name is required";
      InvalidUserFields.Add(nameof(UserDetailModel.LastName));
    }

    // Email 
    if (!IsValidEmail(Email.Text)) {
      valid = false;
      UserErrorMessage.Text = "Invalid email address";
      InvalidUserFields.Add(nameof(UserDetailModel.Email));
    }

    // Password
    if (!String.IsNullOrEmpty(Password.Text) && Password.Text.Length < 6) {
      valid = false;
      UserErrorMessage.Text = "Password is too short";
      InvalidUserFields.Add(nameof(UserDetailModel.Password));
    }

    // Phone
    if (!String.IsNullOrEmpty(Phone.Text) && !ValidatePhoneNumber(Phone.Text)) {
      valid = false;
      UserErrorMessage.Text = "Invalid phone number";
      InvalidUserFields.Add(nameof(UserDetailModel.PhoneNumber));
    }

    OnPropertyChanged(nameof(InvalidUserFields));
    return valid;
  }

  private bool ValidateSubjectFieldValues() {
    bool valid = true;
    InvalidSubjectFields.Clear();

    if (String.IsNullOrEmpty(SubjectName.Text)) {
      valid = false;
      SubjectErrorMessage.Text = "Subject name is required";
      InvalidSubjectFields.Add(nameof(SubjectDetailModel.Name));
    }

    if (String.IsNullOrEmpty(Shortcut.Text)) {
      valid = false;
      SubjectErrorMessage.Text = "Subject shortcut is required";
      InvalidSubjectFields.Add(nameof(SubjectDetailModel.Shortcut));
    }

    OnPropertyChanged(nameof(InvalidSubjectFields));
    return valid;
  }

  public bool ValidatePhoneNumber(string phoneNumber) {
    // Regular expression pattern
    string pattern = @"^\+\d{9,12}$";

    // Create regex object
    Regex regex = new(pattern);

    // Check if the phone number matches the pattern
    return regex.IsMatch(phoneNumber);
  }

  bool IsValidEmail(string? email) {
    if (String.IsNullOrEmpty(email)) return false;

    var trimmedEmail = email.Trim();

    if (trimmedEmail.EndsWith(".")) {
      return false;
    }

    try {
      var addr = new System.Net.Mail.MailAddress(email);
      return addr.Address == trimmedEmail;
    } catch {
      return false;
    }
  }
}