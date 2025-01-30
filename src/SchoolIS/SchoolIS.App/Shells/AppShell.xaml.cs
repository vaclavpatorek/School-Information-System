using SchoolIS.App.Services.Interfaces;

namespace SchoolIS.App.Shells;

public partial class AppShell {
  private readonly INavigationService _navigationService;

  public AppShell(INavigationService navigationService) {
    _navigationService = navigationService;

    InitializeComponent();
  }
}