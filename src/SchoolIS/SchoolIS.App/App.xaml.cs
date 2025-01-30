using SchoolIS.App.Shells;

namespace SchoolIS.App;

public partial class App : Application {
  public App(IServiceProvider serviceProvider) {
    InitializeComponent();

    MainPage = serviceProvider.GetRequiredService<AppShell>();
  }

  protected override Window CreateWindow(IActivationState? activationState) {
    var win = base.CreateWindow(activationState);
    win.Width = 1600;
    win.Height = 900;
    win.MinimumWidth = 1250;
    win.MinimumHeight = 703;
    return win;
  }

  public static object StaticResource(string name) {
    var res = Current!.Resources.TryGetValue(name, out var resource);
    if (res == false) throw new ArgumentException($"Invalid name of color style: `{name}`");

    return resource!;
  }
}