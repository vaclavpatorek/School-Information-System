namespace SchoolIS.App.Views.Components;

public partial class SideBarLayout : ContentView {
  public static readonly BindableProperty PageContentProperty =
    BindableProperty.Create(nameof(PageContent), typeof(View), typeof(SideBarLayout));

  public View PageContent {
    get => (View)GetValue(PageContentProperty);
    set => SetValue(PageContentProperty, value);
  }

  public SideBarLayout() {
    InitializeComponent();
  }
}