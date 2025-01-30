using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using SchoolIS.DAL.Entities;
using Xunit.Abstractions;

namespace SchoolIS.DAL.Tests;

public class DbContextEvaluationTests(ITestOutputHelper output) : DbContextTestsBase(output) {
  [Fact]
  public async Task AddNew_Evaluation_Persisted() {
    //Arrange
    EvaluationEntity entity = new() {
      Id = Guid.Parse("7777902d-7d4f-4213-9cf0-112348f56238"),
      Note = "TEST NOTE",
      Points = 8,
      StudentId = UserSeeds.User.Id,
      ActivityId = ActivitySeeds.Activity.Id,
      Student = null!,
      Activity = null!
    };

    //Act
    SchoolISDbContextSUT.EvaluationEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    EvaluationEntity actualEntities =
      await dbx.EvaluationEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task AddNew_EvaluationWithStudentAndActivity_Persisted() {
    //Arrange
    EvaluationEntity entity = new() {
      Id = Guid.Parse("7777902d-7d4f-4213-9cf0-112348f56238"),
      Note = "TEST NOTE",
      Points = 8,
      StudentId = UserSeeds.User.Id,
      ActivityId = ActivitySeeds.Activity.Id,
      Student = new UserEntity {
        Id = Guid.Parse("e076aed9-fed0-4732-b228-09ad0fd4df91"),
        FirstName = "TEST",
        LastName = "USER",
        PhotoUrl = "TEST_URL.jpg",
        Login = "xTest99",
        PhoneNumber = "0987654321",
        UserType = UserType.Student
      },
      Activity = new ActivityEntity {
        Id = Guid.Parse("af36dffe-dd76-471f-9c15-768c7a71d100"),
        ActivityType = ActivityType.Lecture,
        StartsFrom = DateTime.Parse("15:30:00"),
        EndsAt = DateTime.Parse("15:30:00"),
        CreatorId = UserSeeds.User.Id,
        Creator = null!,
        RoomId = RoomSeeds.Room.Id,
        Room = null!,
        SubjectId = SubjectSeeds.SubjectNoRefs.Id,
        Subject = new SubjectEntity {
          Id = Guid.Parse("7586cd33-072d-44e1-92c5-ed52736e64fe"),
          Name = "TEST SUBJECT",
          Shortcut = "TS",
          Info = "TEST INFO"
        }
      }
    };

    //Act
    SchoolISDbContextSUT.EvaluationEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    EvaluationEntity actualEntities = await dbx.EvaluationEntities
      .Include(i => i.Student)
      .Include(i => i.Activity)
      .ThenInclude(i => i.Subject)
      .SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task GetAll_Evaluations_Retrieved() {
    //Act
    EvaluationEntity[] entities = await SchoolISDbContextSUT.EvaluationEntities
      .ToArrayAsync();

    //Assert
    Assert.Contains(EvaluationSeeds.Evaluation.Id, entities.Select(e => e.Id).ToList());
  }

  [Fact]
  public async Task GetById_Evaluation_TestEvaluationRetrieved() {
    //Act
    EvaluationEntity entity =
      await SchoolISDbContextSUT.EvaluationEntities.SingleAsync(i =>
        i.Id == EvaluationSeeds.Evaluation.Id);


    //Assert
    Assert.Equal(EvaluationSeeds.Evaluation.Id, entity.Id);
  }


  [Fact]
  public async Task GetAll_Evaluation_ForStudentIncludingActivity() {
    //Act
    EvaluationEntity[] evaluationEntity = await SchoolISDbContextSUT.EvaluationEntities
      .Where(i => i.StudentId == UserSeeds.User.Id)
      .Include(i => i.Activity)
      .ToArrayAsync();

    //Assert
    Assert.Contains(EvaluationSeeds.Evaluation.ActivityId,
      evaluationEntity.Select(i => i.ActivityId));
  }

  [Fact]
  public async Task Update_Evaluation_Persisted() {
    //Arrange
    EvaluationEntity baseEntity = EvaluationSeeds.EvaluationUpdate;
    EvaluationEntity entity =
      baseEntity with {
        Points = 9
      };

    //Act
    SchoolISDbContextSUT.EvaluationEntities.Update(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    EvaluationEntity actualEntity =
      await dbx.EvaluationEntities.SingleAsync(i => i.Id == entity.Id);
    Assert.Equal(entity, actualEntity);
  }


  [Fact]
  public async Task DeleteById_Evaluation_EvaluationDeleted() {
    //Arrange
    EvaluationEntity entityBase = EvaluationSeeds.Evaluation;

    //Act
    SchoolISDbContextSUT.Remove(
      SchoolISDbContextSUT.EvaluationEntities.Single(i => i.Id == entityBase.Id));
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(
      await SchoolISDbContextSUT.EvaluationEntities.AnyAsync(i => i.Id == entityBase.Id));
  }
}