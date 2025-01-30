using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL;
using SchoolIS.DAL;
using SchoolIS.DAL.Migrator;
using SchoolIS.DAL.Options;
using SkiaSharp.Views.Maui.Controls.Hosting;
using zoft.MauiExtensions.Controls;

namespace SchoolIS.App;

public static class MauiProgram {
  public static MauiApp CreateMauiApp() {
    var builder = MauiApp.CreateBuilder();
    builder
      .UseSkiaSharp(true)
      .UseMauiApp<App>()
      .UseZoftAutoCompleteEntry()
      .ConfigureFonts(fonts => {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        fonts.AddFont("fa-solid-900.ttf", "FaSolid");
      });

    builder.Logging.AddDebug();

    ConfigureAppSettings(builder);

    builder.Services
      .AddDALServices(GetDALOptions(builder.Configuration))
      .AddBLServices()
      .AddAppServices();

    var app = builder.Build();

    MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
    RegisterRouting(app.Services.GetRequiredService<INavigationService>());

    return app;
  }

  private static void ConfigureAppSettings(MauiAppBuilder builder) {
    var configurationBuilder = new ConfigurationBuilder();

    var assembly = Assembly.GetExecutingAssembly();
    const string appSettingsFilePath = "SchoolIS.App.appsettings.json";
    using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
    if (appSettingsStream is not null) {
      configurationBuilder.AddJsonStream(appSettingsStream);
    }

    var configuration = configurationBuilder.Build();
    builder.Configuration.AddConfiguration(configuration);
  }

  private static void RegisterRouting(INavigationService navigationService) {
    foreach (var route in navigationService.Routes) {
      Routing.RegisterRoute(route.Route, route.ViewType);
    }
  }

  private static DALOptions GetDALOptions(IConfiguration configuration) {
    DALOptions dalOptions = new() {
      DatabaseDirectory = FileSystem.AppDataDirectory,
      DatabaseName = "SchoolIS.db"
    };
    configuration.GetSection("SchoolIS:DAL").Bind(dalOptions);
    return dalOptions;
  }

  private static void MigrateDb(IDbMigrator migrator) => migrator.Migrate();
}