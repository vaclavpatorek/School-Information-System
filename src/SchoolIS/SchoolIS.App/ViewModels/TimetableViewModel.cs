using SchoolIS.App.Services.Interfaces;

namespace SchoolIS.App.ViewModels;

public class TimetableViewModel(
  IMessengerService messengerService,
  INavigationService navigation,
  IAuthenticationService authenticationService)
  : ViewModelBase(messengerService, navigation, authenticationService){}