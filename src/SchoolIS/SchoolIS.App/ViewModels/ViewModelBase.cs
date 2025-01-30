using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SchoolIS.App.Messages;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL.Models;

namespace SchoolIS.App.ViewModels;

public abstract partial class ViewModelBase
  : ObservableRecipient, IViewModel, IRecipient<SidebarTabSelectedMessage>, IRecipient<UserLoginMessage> {
  private bool _isRefreshRequired = true;

  protected readonly IMessengerService MessengerService;
  protected readonly INavigationService NavigationService;
  protected readonly IAuthenticationService AuthenticationService;

  [ObservableProperty] private string _selectedTab = "Timetable";
  [ObservableProperty] private bool _loading = true;

  public UserDetailModel AuthenticatedUser {
    get => AuthenticationService.GetLoggedInUser();
  }

  protected ViewModelBase(IMessengerService messengerService, INavigationService navigationService,
    IAuthenticationService authenticationService)
    : base(messengerService.Messenger) {
    MessengerService = messengerService;
    NavigationService = navigationService;
    AuthenticationService = authenticationService;
    IsActive = true;
  }

  public async Task OnAppearingAsync() {
    Loading = true;

    if (_isRefreshRequired) {
      await LoadDataAsync();
      _isRefreshRequired = false;
    }

    Loading = false;
  }

  protected virtual Task LoadDataAsync() => Task.CompletedTask;

  public void Receive(SidebarTabSelectedMessage message) {
    SelectedTab = message.TabName;
  }

  public void Receive(UserLoginMessage message) {
    OnPropertyChanged(nameof(AuthenticatedUser));
  }

  [RelayCommand]
  public async Task GoToAsync(string page) {
    await NavigationService.GoToAsync($"//{page.ToLower()}");
    SetActiveSidebarTab(page);
  }

  [RelayCommand]
  public async Task GoToSubject(Guid subjectId) {
    await NavigationService.GoToAsync($"//subjects?{nameof(SubjectDetailModel.Id)}={subjectId}");
    SetActiveSidebarTab(subjectId.ToString());
  } 

  [RelayCommand]
  public async Task GoToActivity(ActivityListModel activity) {
    await NavigationService.GoToAsync($"//activities?{nameof(ActivityDetailModel.Id)}={activity.Id}");
    SetActiveSidebarTab(activity.SubjectId.ToString());
  } 

  [RelayCommand]
  public async Task Logout() {
    await NavigationService.GoToAsync($"//login");
    AuthenticationService.LogoutUser();
  }

  [RelayCommand]
  public async Task Refresh() {
    await LoadDataAsync();

    OnPropertyChanged(nameof(AuthenticatedUser));
  }

  private void SetActiveSidebarTab(string page) {
    MessengerService.Send(new SidebarTabSelectedMessage { TabName = page });
    SelectedTab = page;
  }

}