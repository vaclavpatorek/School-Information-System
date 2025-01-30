using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL;
using SchoolIS.DAL.Migrator;
using SchoolIS.DAL.Options;

namespace ScholIS.DAL.Migrator;

public class DbMigrator(IDbContextFactory<SchoolISDbContext> dbContextFactory, DALOptions options)
  : IDbMigrator {
  public void Migrate() => MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();

  public async Task MigrateAsync(CancellationToken cancellationToken) {
    await using SchoolISDbContext dbContext =
      await dbContextFactory.CreateDbContextAsync(cancellationToken);

    if (options.RecreateDatabaseEachTime) {
      await dbContext.Database.EnsureDeletedAsync(cancellationToken);
    }

    // Ensures that database is created applying the latest state
    // Application of migration later on may fail
    // If you want to use migrations, you should create database by calling  dbContext.Database.MigrateAsync(cancellationToken) instead
    await dbContext.Database.EnsureCreatedAsync(cancellationToken);
  }
}