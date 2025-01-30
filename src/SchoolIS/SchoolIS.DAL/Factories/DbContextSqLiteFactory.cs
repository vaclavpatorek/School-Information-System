using Microsoft.EntityFrameworkCore;

namespace SchoolIS.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<SchoolISDbContext> {
  private readonly DbContextOptionsBuilder<SchoolISDbContext> _contextOptionsBuilder = new();
  private readonly bool _seedTestingData;

  public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false) {
    _seedTestingData = seedTestingData;

    ////May be helpful for ad-hoc testing, not drop in replacement, needs some more configuration.
    //builder.UseSqlite($"Data Source =:memory:;");
    _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");

    ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
    //_contextOptionsBuilder.EnableSensitiveDataLogging();
    //_contextOptionsBuilder.LogTo(Console.WriteLine);
  }

  public SchoolISDbContext CreateDbContext() {
    return new SchoolISDbContext(_contextOptionsBuilder.Options, _seedTestingData);
  }
}