using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL;

namespace SchoolIS.Common.Tests.Factories;

public class DbContextSqLiteTestingFactory(string databaseName, bool seedTestingData = false)
  : IDbContextFactory<SchoolISDbContext> {
  public SchoolISDbContext CreateDbContext() {
    DbContextOptionsBuilder<SchoolISDbContext> builder = new();
    builder.UseSqlite($"Data Source={databaseName};Cache=Shared");

    // builder.LogTo(Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
    // builder.EnableSensitiveDataLogging();

    return new SchoolISTestingDbContext(builder.Options, seedTestingData);
  }
}