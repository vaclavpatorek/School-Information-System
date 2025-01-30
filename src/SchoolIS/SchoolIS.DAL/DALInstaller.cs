using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScholIS.DAL.Migrator;
using SchoolIS.DAL.Factories;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.Migrator;
using SchoolIS.DAL.Options;

namespace SchoolIS.DAL;

public static class DALInstaller {
  public static IServiceCollection AddDALServices(this IServiceCollection services,
    DALOptions options) {
    services.AddSingleton(options);

    if (options is null) {
      throw new InvalidOperationException("No persistence provider configured");
    }

    if (string.IsNullOrEmpty(options.DatabaseDirectory)) {
      throw new InvalidOperationException($"{nameof(options.DatabaseDirectory)} is not set");
    }

    if (string.IsNullOrEmpty(options.DatabaseName)) {
      throw new InvalidOperationException($"{nameof(options.DatabaseName)} is not set");
    }

    services.AddSingleton<IDbContextFactory<SchoolISDbContext>>(_ =>
      new DbContextSqLiteFactory(options.DatabaseFilePath, options?.SeedDemoData ?? false));
    services.AddSingleton<IDbMigrator, DbMigrator>();

    services.AddSingleton<ActivityEntityMapper>();
    services.AddSingleton<HasSubjectEntityMapper>();
    services.AddSingleton<EvaluationEntityMapper>();
    services.AddSingleton<RoomEntityMapper>();
    services.AddSingleton<SubjectEntityMapper>();
    services.AddSingleton<UserEntityMapper>();

    return services;
  }
}