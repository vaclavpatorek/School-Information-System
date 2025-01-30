using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using SchoolIS.DAL.Entities;
using Xunit.Abstractions;

namespace SchoolIS.DAL.Tests;

public class DbContextActivityTests(ITestOutputHelper output) : DbContextTestsBase(output) {
  [Fact]
  public async Task AddNew_Activity_Persisted() {
    //Arrange
    ActivityEntity entity = new() {
      Id = Guid.Parse("0011902d-7d4f-4213-9cf0-112348f56238"),
      ActivityType = ActivityType.Lecture,
      StartsFrom = DateTime.Parse("15:30:00"),
      EndsAt = DateTime.Parse("15:30:00"),
      CreatorId = UserSeeds.User.Id,
      Creator = null!,
      RoomId = RoomSeeds.Room.Id,
      Room = null!,
      SubjectId = SubjectSeeds.SubjectNoRefs.Id,
      Subject = null!
    };

    //Act
    SchoolISDbContextSUT.ActivityEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    ActivityEntity actualEntities = await dbx.ActivityEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task AddNew_ActivityWithAllRelations_Persisted() {
    //Arrange
    ActivityEntity entity = new() {
      Id = Guid.Parse("0011902d-7d4f-4213-9cf0-112348f56238"),
      ActivityType = ActivityType.Lecture,
      StartsFrom = DateTime.Parse("15:30:00"),
      EndsAt = DateTime.Parse("15:30:00"),
      CreatorId = UserSeeds.User.Id,
      Creator = new UserEntity {
        Id = Guid.Parse("e076aed9-fed0-4732-b228-09ad0fd4df91"),
        FirstName = "TEST",
        LastName = "USER",
        PhotoUrl = "TEST_URL.jpg",
        Login = "xTest99",
        PhoneNumber = "0987654321",
        UserType = UserType.Teacher
      },
      RoomId = RoomSeeds.Room.Id,
      Room = new RoomEntity {
        Id = Guid.Parse("7e21dc18-ac88-49ba-ac83-325f25e1199d"),
        Name = "TEST ROOM"
      },
      SubjectId = SubjectSeeds.SubjectNoRefs.Id,
      Subject = new SubjectEntity {
        Id = Guid.Parse("7586cd33-072d-44e1-92c5-ed52736e64fe"),
        Name = "TEST SUBJECT",
        Shortcut = "TS",
        Info = "TEST INFO"
      }
    };

    //Act
    SchoolISDbContextSUT.ActivityEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    ActivityEntity actualEntities = await dbx.ActivityEntities
      .Include(i => i.Creator)
      .Include(i => i.Room)
      .Include(i => i.Subject)
      .SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task GetAll_Activity_ContainsActivity() {
    //Act
    ActivityEntity[] entities = await SchoolISDbContextSUT.ActivityEntities
      .ToArrayAsync();

    //Assert
    Assert.Contains(ActivitySeeds.Activity.Id, entities.Select(i => i.Id));
  }


  [Fact]
  public async Task GetById_Activity_TestActivityRetrieved() {
    ActivityEntity expectedActivity = ActivitySeeds.Activity;

    //Act
    ActivityEntity entity =
      await SchoolISDbContextSUT.ActivityEntities
        .SingleAsync(i => i.Id == expectedActivity.Id);


    //Assert
    DeepAssert.Equal(expectedActivity.Id, entity.Id);
  }

  [Fact]
  public async Task Update_Activity_Persisted() {
    //Arrange
    ActivityEntity baseEntity = ActivitySeeds.ActivityDelete;
    ActivityEntity entity =
      baseEntity with {
        ActivityType = ActivityType.Meeting
      };

    //Act
    SchoolISDbContextSUT.ActivityEntities.Update(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    ActivityEntity actualEntity = await dbx.ActivityEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntity);
  }

  [Fact]
  public async Task Delete_Activity_ActivityDeleted() {
    //Arrange
    ActivityEntity entityBase = ActivitySeeds.ActivityDelete;

    //Act
    SchoolISDbContextSUT.ActivityEntities.Remove(entityBase);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.ActivityEntities.AnyAsync(i => i.Id == entityBase.Id));
  }

  [Fact]
  public async Task DeleteById_Activity_ActivityDeleted() {
    //Arrange
    ActivityEntity entityBase = ActivitySeeds.Activity;

    //Act
    SchoolISDbContextSUT.Remove(
      SchoolISDbContextSUT.ActivityEntities.Single(i => i.Id == entityBase.Id));
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.ActivityEntities.AnyAsync(i => i.Id == entityBase.Id));
    Assert.False(
      await SchoolISDbContextSUT.EvaluationEntities.AnyAsync(i =>
        i.Id == EvaluationSeeds.Evaluation.Id));
  }
}