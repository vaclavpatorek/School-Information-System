using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Factories;
using SchoolIS.DAL;
using SchoolIS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using SchoolIS.BL.Mappers;

namespace SchoolIS.BL.Tests;

public class FacadeTestsBase : IAsyncLifetime {
  protected UnitOfWorkFactory UnitOfWorkFactory { get; }
  protected RoomModelMapper RoomModelMapper { get; }
  protected SubjectModelMapper SubjectModelMapper { get; }
  protected ActivityModelMapper ActivityModelMapper { get; }
  protected UserModelMapper UserModelMapper { get; }
  protected EvaluationModelMapper EvaluationModelMapper { get; }
  protected HasSubjectModelMapper HasSubjectModelMapper { get; }

  protected FacadeTestsBase(ITestOutputHelper output) {
    XUnitTestOutputConverter converter = new(output);
    Console.SetOut(converter);

    DbContextFactory =
      new DbContextSqLiteTestingFactory(Guid.NewGuid().ToString(), seedTestingData: true);

    RoomModelMapper = new RoomModelMapper();
    EvaluationModelMapper = new EvaluationModelMapper();
    ActivityModelMapper = new ActivityModelMapper(EvaluationModelMapper);
    HasSubjectModelMapper = new HasSubjectModelMapper();
    UserModelMapper = new UserModelMapper(HasSubjectModelMapper);
    SubjectModelMapper = new SubjectModelMapper(HasSubjectModelMapper);
    UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
  }

  protected IDbContextFactory<SchoolISDbContext> DbContextFactory { get; }


  public async Task InitializeAsync() {
    await using var dbx = await DbContextFactory.CreateDbContextAsync();
    await dbx.Database.EnsureDeletedAsync();
    await dbx.Database.EnsureCreatedAsync();
  }

  public async Task DisposeAsync() {
    await using var dbx = await DbContextFactory.CreateDbContextAsync();
    await dbx.Database.EnsureDeletedAsync();
  }
}