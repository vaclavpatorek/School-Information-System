using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using SchoolIS.DAL.Entities;
using Xunit.Abstractions;

namespace SchoolIS.DAL.Tests;

public class DbContextSubjectTests(ITestOutputHelper output) : DbContextTestsBase(output) {
  [Fact]
  public async Task AddNew_Subject_Persisted() {
    //Arrange
    SubjectEntity entity = new() {
      Id = Guid.Parse("0013902d-7d4f-4213-9cf0-112348f56238"),
      Name = "TEST SUBJECT",
      Shortcut = "TS",
      Info = null
    };

    //Act
    SchoolISDbContextSUT.SubjectEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    SubjectEntity actualEntities = await dbx.SubjectEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task AddNew_SubjectWithActivities_Persisted() {
    //Arrange
    SubjectEntity entity = new() {
      Id = Guid.Parse("0013902d-7d4f-4213-9cf0-112348f56238"),
      Name = "TEST SUBJECT",
      Shortcut = "TS",
      Info = null,
      Activities = new List<ActivityEntity> {
        new() {
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
        },
        new() {
          Id = Guid.Parse("65337507-5e37-4093-bb63-5abf87aa5843"),
          ActivityType = ActivityType.Exercise,
          StartsFrom = DateTime.Parse("15:30:00"),
          EndsAt = DateTime.Parse("15:30:00"),
          CreatorId = UserSeeds.User.Id,
          Creator = null!,
          RoomId = RoomSeeds.Room.Id,
          Room = null!,
          SubjectId = SubjectSeeds.SubjectNoRefs.Id,
          Subject = null!
        }
      }
    };

    //Act
    SchoolISDbContextSUT.SubjectEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    SubjectEntity actualEntities = await dbx.SubjectEntities
      .Include(i => i.Activities)
      .SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }


  [Fact]
  public async Task GetAll_Subject_ContainsSubjectWithNoReferences() {
    //Act
    SubjectEntity[] entities = await SchoolISDbContextSUT.SubjectEntities
      .ToArrayAsync();

    //Assert
    DeepAssert.Contains(SubjectSeeds.SubjectNoRefs, entities);
  }

  [Fact]
  public async Task GetById_Subject_TestSubjectWithNoReferencesRetrieved() {
    //Act
    SubjectEntity entity =
      await SchoolISDbContextSUT.SubjectEntities
        .SingleAsync(i => i.Id == SubjectSeeds.SubjectNoRefs.Id);


    //Assert
    DeepAssert.Equal(SubjectSeeds.SubjectNoRefs, entity);
  }

  [Fact]
  public async Task GetById_Subject_TestSubjectWithTeachersRetrieved() {
    SubjectEntity expectedSubject = SubjectSeeds.Subject;

    //Act
    SubjectEntity entity =
      await SchoolISDbContextSUT.SubjectEntities
        .Include(i => i.Users)
        .SingleAsync(i => i.Id == expectedSubject.Id);


    //Assert
    Assert.Contains(UserSeeds.User.Id, entity.Users.Select(i => i.UserId));
  }

  [Fact]
  public async Task Update_Subject_Persisted() {
    //Arrange
    SubjectEntity baseEntity = SubjectSeeds.SubjectNoRefs;
    SubjectEntity entity =
      baseEntity with {
        Name = baseEntity + "Updated",
        Shortcut = baseEntity + "Updated"
      };

    //Act
    SchoolISDbContextSUT.SubjectEntities.Update(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    SubjectEntity actualEntity = await dbx.SubjectEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntity);
  }

  [Fact]
  public async Task Delete_Subject_SubjectDeleted() {
    //Arrange
    SubjectEntity entityBase = SubjectSeeds.SubjectNoRefs;

    //Act
    SchoolISDbContextSUT.SubjectEntities.Remove(entityBase);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.SubjectEntities.AnyAsync(i => i.Id == entityBase.Id));
  }

  [Fact]
  public async Task DeleteById_Subject_SubjectDeleted() {
    //Arrange
    SubjectEntity entityBase = SubjectSeeds.Subject;

    //Act
    SchoolISDbContextSUT.Remove(
      SchoolISDbContextSUT.SubjectEntities.Single(i => i.Id == entityBase.Id));
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.SubjectEntities.AnyAsync(i => i.Id == entityBase.Id));
    Assert.False(
      await SchoolISDbContextSUT.HasSubjectEntities.AnyAsync(i => i.Id == HasSubjectSeeds.HasSubject.Id));
    Assert.False(
      await SchoolISDbContextSUT.ActivityEntities.AnyAsync(i =>
        i.Id == ActivitySeeds.Activity.Id));
    Assert.False(
      await SchoolISDbContextSUT.ActivityEntities.AnyAsync(
        i => i.Id == ActivitySeeds.Activity2.Id));
  }
}