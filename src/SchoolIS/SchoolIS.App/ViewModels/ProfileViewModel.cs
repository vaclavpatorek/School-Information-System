using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolIS.App.Messages;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;

namespace SchoolIS.App.ViewModels;

public partial class ProfileViewModel(
  IMessengerService messengerService,
  INavigationService navigation,
  IUserFacade userFacade,
  ISubjectFacade subjectFacade,
  IAuthenticationService authenticationService)
  : ViewModelBase(messengerService, navigation, authenticationService) {
  [ObservableProperty] private string _errorMessage = "";
  [ObservableProperty] private ObservableCollection<SubjectDetailModel> _subjects = [];
  [ObservableProperty] private ObservableCollection<Guid> _userSubjects = [];
  [ObservableProperty] private ObservableHashSet<string> _dataChanged = [];

  [ObservableProperty] private string _email = string.Empty;
  [ObservableProperty] private string _phone = string.Empty;
  [ObservableProperty] private string? _photo = string.Empty;


  protected override async Task LoadDataAsync() {
    AuthenticationService.LoginWithoutPasswordValidation((await userFacade.GetAsync(AuthenticatedUser.Id))!);

    ObservableCollection<SubjectDetailModel> subjects = [];

    foreach (var hasSubject in AuthenticatedUser.Subjects) {
      SubjectDetailModel? subject = await subjectFacade.GetAsync(hasSubject.SubjectId);

      if (subject == null) {
        Trace.WriteLine("Invalid hasSubject subject id.");
        continue;
      }

      subjects.Add(subject);
    }

    Subjects = subjects;
    SetUserData();
  }


  // Save edited user configuration
  [RelayCommand]
  private async Task EditUserAsync() {
    try {
      UserDetailModel userDetailModel = await userFacade.SaveAsync(AuthenticatedUser with {
        Email = Email,
        PhoneNumber = Phone,
        PhotoUrl = Photo,
        Subjects = null!
      });

      AuthenticationService.LoginWithoutPasswordValidation(userDetailModel);
      MessengerService.Send(new UserLoginMessage());
      DataChanged.Clear();
      OnPropertyChanged(nameof(DataChanged));
    } catch (InvalidOperationException e) {
      ErrorMessage = e.Message;
    }
  }

  [RelayCommand]
  private void UpdateEmail(string text) {
    if (text != (AuthenticatedUser.Email ?? string.Empty))
      DataChanged.Add(nameof(AuthenticatedUser.Email));
    else DataChanged.Remove(nameof(AuthenticatedUser.Email));
    OnPropertyChanged(nameof(DataChanged));
  }

  [RelayCommand]
  private void UpdatePhone(string text) {
    if (text != (AuthenticatedUser.PhoneNumber ?? string.Empty))
      DataChanged.Add(nameof(AuthenticatedUser.PhoneNumber));
    else DataChanged.Remove(nameof(AuthenticatedUser.PhoneNumber));
    OnPropertyChanged(nameof(DataChanged));
  }

  [RelayCommand]
  private void UpdatePhoto(string text) {
    Photo = text;
    if (text != (AuthenticatedUser.PhotoUrl ?? string.Empty))
      DataChanged.Add(nameof(AuthenticatedUser.PhotoUrl));
    else DataChanged.Remove(nameof(AuthenticatedUser.PhotoUrl));
    OnPropertyChanged(nameof(DataChanged));
  }

  [RelayCommand]
  private void CancelEditing() {
    SetUserData();
  }

  private void SetUserData() {
    DataChanged.Clear();
    Email = AuthenticatedUser.Email ?? string.Empty;
    Phone = AuthenticatedUser.PhoneNumber ?? string.Empty;
    Photo = AuthenticatedUser.PhotoUrl;
    OnPropertyChanged(nameof(DataChanged));
  }
}