using System.Security.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolIS.App.Messages;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;

namespace SchoolIS.App.ViewModels;

public partial class LoginViewModel(
  IUserFacade userFacade,
  IMessengerService messengerService,
  INavigationService navigation,
  IAuthenticationService authenticationService)
  : ViewModelBase(messengerService, navigation, authenticationService) {
  [ObservableProperty] private string _errorText = string.Empty;
  [ObservableProperty] private string _userLogin = string.Empty;
  [ObservableProperty] private string _password = string.Empty;


  [RelayCommand]
  public async Task Login() {
    // Try to authenticate admin
    if (AuthenticationService.IsAdmin(UserLogin, Password)) {
      await GoToAsync("Admin");
      ClearCredentials();
      return;
    }

    UserDetailModel? user = await userFacade.GetUserByLogin(UserLogin);
    await TryLoginUser(user);
  }

  private async Task TryLoginUser(UserDetailModel? user) {
    try {
      AuthenticationService.LoginUser(user, Password);
      await GoToAsync("Profile");

      // Notify view models about change in authenticated user
      MessengerService.Send(new UserLoginMessage());

      ClearCredentials();
    } catch (AuthenticationException e) {
      ErrorText = e.Message;
    }
  }

  private void ClearCredentials() {
    Password = "";
    UserLogin = "";
    ErrorText = "";
  }
}