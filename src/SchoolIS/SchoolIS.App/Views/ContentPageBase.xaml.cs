using SchoolIS.App.ViewModels;

namespace SchoolIS.App.Views;

public partial class ContentPageBase : ContentPage {
  protected IViewModel ViewModel { get; }

  public ContentPageBase(IViewModel viewModel) {
    InitializeComponent();

    BindingContext = ViewModel = viewModel;
  }

  protected override async void OnAppearing() {
    base.OnAppearing();

    await ViewModel.OnAppearingAsync();
  }
}