using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using SchoolIS.App.ViewModels;
using SchoolIS.BL.Models;

namespace SchoolIS.App.Views;

public partial class ProfilePage : ContentPageBase {
  readonly ProfileViewModel _profileViewModel;
  public ObservableCollection<string> InvalidFields { get; set; } = [];

  public ProfilePage(ProfileViewModel profileViewModel) : base(profileViewModel) {
    this._profileViewModel = profileViewModel;
    InitializeComponent();
  }

  private bool _photoEditMode = false;

  public bool PhotoEditMode {
    get => _photoEditMode;
    set {
      if (value == _photoEditMode) return;
      _photoEditMode = value;
      OnPropertyChanged();
    }
  }

  [RelayCommand]
  private void EnablePhotoEditMode() {
    PhotoEditMode = true;
  }

  [RelayCommand]
  private void CancelPhotoEditMode() {
    PhotoEditMode = false;
    PhotoUrl.Text = "";
    ErrorMessage.Text = "";
  }

  [RelayCommand]
  private async Task ValidateAndTrySave() {
    if (ValidateUserFieldValues()) {
      await _profileViewModel.EditUserCommand.ExecuteAsync(null);
      ErrorMessage.Text = "";
    }
  }

  [RelayCommand]
  private async Task UpdatePhotoAsync(string photo) {
    InvalidFields.Remove(nameof(UserDetailModel.PhotoUrl));

    if (await CheckIfImageExists(photo)) {
      _profileViewModel.UpdatePhotoCommand.Execute(photo);
      PhotoEditMode = false;
      PhotoUrl.Text = "";
      ErrorMessage.Text = "";
    }

    OnPropertyChanged(nameof(InvalidFields));
  }

  private async Task<bool> CheckIfImageExists(string url) {
    using var httpClient = new HttpClient();
    try {
      using var response = await httpClient.GetAsync(url);

      if (response.StatusCode == HttpStatusCode.OK) return true;

      ErrorMessage.Text = "Invalid image URL";
    } catch (Exception ex) {
      ErrorMessage.Text = ex.Message;
    }

    InvalidFields.Add(nameof(UserDetailModel.PhotoUrl));
    return false;
  }

  [RelayCommand]
  private void CancelEditing() {
    _profileViewModel.CancelEditingCommand.Execute(null);
    ErrorMessage.Text = "";
    PhotoUrl.Text = "";
    PhotoEditMode = false;
    InvalidFields.Clear();
  }

  [RelayCommand]
  private void UpdateEmail(string email) {
    InvalidFields.Remove(nameof(UserDetailModel.Email));
    _profileViewModel.UpdateEmailCommand.Execute(email);
    OnPropertyChanged(nameof(InvalidFields));
  }

  [RelayCommand]
  private void UpdatePhone(string phone) {
    InvalidFields.Remove(nameof(UserDetailModel.PhoneNumber));
    _profileViewModel.UpdatePhoneCommand.Execute(phone);
    OnPropertyChanged(nameof(InvalidFields));
  }

  private bool ValidateUserFieldValues() {
    bool valid = true;
    InvalidFields.Clear();

    // Email 
    if (!IsValidEmail(EmailField.Text)) {
      valid = false;
      ErrorMessage.Text = "Invalid email address";
      InvalidFields.Add(nameof(UserDetailModel.Email));
    }

    // Phone
    if (!String.IsNullOrEmpty(Phone.Text) &&
        !ValidatePhoneNumber(Phone.Text)) {
      valid = false;
      ErrorMessage.Text = "Invalid phone number";
      InvalidFields.Add(nameof(UserDetailModel.PhoneNumber));
    }

    OnPropertyChanged(nameof(InvalidFields));
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