using CommunityToolkit.Mvvm.Messaging;
using SchoolIS.App.Services;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.App.Shells;
using SchoolIS.App.ViewModels;
using SchoolIS.App.Views;
using SchoolIS.App.Views.Components;

namespace SchoolIS.App;

public static class AppInstaller {
  public static IServiceCollection AddAppServices(this IServiceCollection services) {
    services.AddSingleton<AppShell>();

    services.AddSingleton<IMessenger>(_ => StrongReferenceMessenger.Default);
    services.AddSingleton<IMessengerService, MessengerService>();
    services.AddSingleton<IAuthenticationService, AuthenticationService>();

    services.AddSingleton<IAlertService, AlertService>();

    services.Scan(selector => selector
      .FromAssemblyOf<App>()
      .AddClasses(filter => filter.AssignableTo<ContentPageBase>())
      .AsSelf()
      .WithTransientLifetime());

    services.AddSingleton<AppBar>();

    services.Scan(selector => selector
      .FromAssemblyOf<App>()
      .AddClasses(filter => filter.AssignableTo<ViewModelBase>())
      .AsSelf()
      .WithTransientLifetime());

    services.AddTransient<INavigationService, NavigationService>();

    return services;
  }
}