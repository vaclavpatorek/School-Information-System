using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Enums;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using SchoolIS.DAL.Entities;
using Xunit.Abstractions;

namespace SchoolIS.BL.Tests;

public sealed class ActivityFacadeTests : FacadeTestsBase {
  private readonly IActivityFacade _activityFacadeSUT;

  public ActivityFacadeTests(ITestOutputHelper output) : base(output) {
    _activityFacadeSUT =
      new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
  }


  [Fact]
  public async Task Create_WithNonExistingReferences_DoesNotThrow() {
    var model = ActivityDetailModel.Empty;

    await Assert.ThrowsAnyAsync<InvalidOperationException>(
      () => _activityFacadeSUT.SaveAsync(model));
  }

  [Fact]
  public async Task Create_WithExistingReferences_DoesNotThrow() {
    // Arrange
    var model = new ActivityDetailModel {
      Id = Guid.Empty,
      EndsAt = DateTime.UtcNow,
      StartsFrom = DateTime.UtcNow,
      ActivityType = ActivityType.Lecture,
      CreatorFirstName = "FIRST",
      CreatorLastName = "LAST",
      RoomName = "TEST ROOM",
      CreatorId = UserSeeds.User.Id,
      SubjectId = SubjectSeeds.Subject.Id,
      RoomId = RoomSeeds.Room.Id,
    };

    // Act
    await _activityFacadeSUT.SaveAsync(model);
  }

  [Fact]
  public async Task GetAll_Single_SeededActivity() {
    // Act
    var activities = await _activityFacadeSUT.GetAsync();
    var activity = activities.Single(i => i.Id == ActivitySeeds.Activity.Id);

    // Assert
    DeepAssert.Equal(ActivityModelMapper.MapToListModel(ActivitySeeds.Activity), activity);
  }

  [Fact]
  public async Task GetById_SeededActivity() {
    // Act
    var activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.Activity.Id);

    // Assert
    DeepAssert.Equal(ActivityModelMapper.MapToDetailModel(ActivitySeeds.Activity), activity);
  }

  [Fact]
  public async Task GetById_NonExistent() {
    // Act
    var activity = await _activityFacadeSUT.GetAsync(ActivitySeeds.EmptyActivity.Id);

    // Assert
    Assert.Null(activity);
  }

  [Fact]
  public async Task GetActivities_ByUserId() {
    // Act
    var filterActivitiesByDateAndUser =
      await _activityFacadeSUT.FilterActivitiesByDateAndUser(UserSeeds.User.Id);

    // Assert
    var expected = new List<ActivityEntity>([
        ActivitySeeds.Activity3, ActivitySeeds.Activity2, ActivitySeeds.Activity
      ])
      .Select(ActivityModelMapper.MapToListModel);

    DeepAssert.Equal(expected, filterActivitiesByDateAndUser);
  }


  [Fact]
  public async Task GetActivities_BySubjectId() {
    // Act
    var filterActivitiesByDateAndUser =
      await _activityFacadeSUT.FilterActivitiesByDateAndSubject(SubjectSeeds.Subject.Id);

    // Assert
    var expected = new List<ActivityEntity>([ActivitySeeds.Activity2, ActivitySeeds.Activity])
      .Select(ActivityModelMapper.MapToListModel);

    DeepAssert.Equal(expected, filterActivitiesByDateAndUser);
  }


  [Fact]
  public async Task GetActivities_ByDate_Found() {
    // Act
    var filterActivitiesByDateAndUser =
      await _activityFacadeSUT.FilterActivitiesByDateAndUser(UserSeeds.User.Id,
        DateTime.Parse("May 8, 2024"), DateTime.Parse("May 9, 2024"));

    var expected =
      new List<ActivityEntity>([ActivitySeeds.Activity2]).Select(ActivityModelMapper
        .MapToListModel);

    // Assert
    DeepAssert.Equal(expected, filterActivitiesByDateAndUser);
  }

  [Fact]
  public async Task GetActivities_ByUserId_And_Date_NotFound() {
    // Act
    var filterActivitiesByDateAndUser =
      await _activityFacadeSUT.FilterActivitiesByDateAndUser(UserSeeds.User.Id,
        DateTime.Parse("May 10, 2024"), DateTime.Parse("May 12, 2024"));

    // Assert
    Assert.Empty(filterActivitiesByDateAndUser);
  }


  [Fact]
  public async Task Delete_ActivityWithEvaluations_DoesNotThrow() {
    //Act & Assert
    await _activityFacadeSUT.DeleteAsync(ActivitySeeds.Activity.Id);
  }

  [Fact]
  public async Task SeededActivity_DeleteById_Deleted() {
    // Arrange
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    // Act
    await _activityFacadeSUT.DeleteAsync(ActivitySeeds.Activity.Id);

    // Assert
    Assert.False(
      await dbxAssert.ActivityEntities.AnyAsync(i => i.Id == ActivitySeeds.Activity.Id));

    // Cascade deletion of all related evaluations
    Assert.False(
      await dbxAssert.EvaluationEntities.AnyAsync(i => i.ActivityId == ActivitySeeds.Activity.Id));
  }

  [Fact]
  public async Task NewActivity_InsertOrUpdate_ActivityAdded() {
    //Arrange
    var model = new ActivityDetailModel {
      StartsFrom = DateTime.Parse("2024-03-22 11:14:12"),
      EndsAt = DateTime.Parse("2024-03-22 14:00:00"),
      ActivityType = ActivityType.Meeting,
      CreatorFirstName = UserSeeds.User.FirstName,
      CreatorLastName = UserSeeds.User.LastName,
      CreatorId = UserSeeds.User.Id,
      SubjectId = SubjectSeeds.Subject.Id,
      SubjectName = SubjectSeeds.Subject.Name,
      SubjectShortcut = SubjectSeeds.Subject.Shortcut,
      RoomId = RoomSeeds.Room.Id,
      RoomName = RoomSeeds.Room.Name
    };

    // Act
    ActivityDetailModel createdModel = await _activityFacadeSUT.SaveAsync(model);
    model.Id = createdModel.Id;

    // Assert
    DeepAssert.Equal(model, createdModel);
  }

  [Fact]
  public async Task SeededActivity_InsertOrUpdate_ActivityUpdated() {
    //Arrange
    var activity = new ActivityDetailModel {
      Id = ActivitySeeds.Activity.Id,
      StartsFrom = ActivitySeeds.Activity.StartsFrom,
      EndsAt = ActivitySeeds.Activity.EndsAt,
      ActivityType = ActivitySeeds.Activity.ActivityType,
      CreatorFirstName = ActivitySeeds.Activity.Creator.FirstName,
      CreatorLastName = ActivitySeeds.Activity.Creator.LastName,
      CreatorId = ActivitySeeds.Activity.CreatorId,
      SubjectId = ActivitySeeds.Activity.SubjectId,
      SubjectName = SubjectSeeds.Subject.Name,
      SubjectShortcut = SubjectSeeds.Subject.Shortcut,
      RoomId = ActivitySeeds.Activity.RoomId,
      RoomName = ActivitySeeds.Activity.Room.Name
    };

    activity.ActivityType = ActivityType.Exercise;

    //Act
    await _activityFacadeSUT.SaveAsync(activity);

    //Assert
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
    var activityFromDb = await dbxAssert.ActivityEntities
      .Include(i => i.Creator)
      .Include(i => i.Subject)
      .Include(i => i.Room)
      .SingleAsync(i => i.Id == activity.Id);

    DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
  }
}