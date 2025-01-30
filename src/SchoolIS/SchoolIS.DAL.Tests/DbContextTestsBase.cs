using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Factories;
using Xunit.Abstractions;

namespace SchoolIS.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime {
  protected DbContextTestsBase(ITestOutputHelper output) {
    XUnitTestOutputConverter converter = new(output);
    Console.SetOut(converter);

    DbContextFactory =
      new DbContextSqLiteTestingFactory(Guid.NewGuid().ToString(), true);
    SchoolISDbContextSUT = DbContextFactory.CreateDbContext();
  }

  protected IDbContextFactory<SchoolISDbContext> DbContextFactory { get; }
  protected SchoolISDbContext SchoolISDbContextSUT { get; }


  public async Task InitializeAsync() {
    await SchoolISDbContextSUT.Database.EnsureDeletedAsync();
    await SchoolISDbContextSUT.Database.EnsureCreatedAsync();
  }

  public async Task DisposeAsync() {
    await SchoolISDbContextSUT.Database.EnsureDeletedAsync();
    await SchoolISDbContextSUT.DisposeAsync();
  }
}