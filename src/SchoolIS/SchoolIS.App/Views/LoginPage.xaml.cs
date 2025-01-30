using SchoolIS.App.ViewModels;

namespace SchoolIS.App.Views;

public partial class LoginPage : ContentPageBase {
  public LoginPage(LoginViewModel loginViewModel) : base(loginViewModel) {
    InitializeComponent();
  }
}