using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace SchoolIS.BL.Tests;

public sealed class HasSubjectFacadeTests : FacadeTestsBase {
  private readonly IHasSubjectFacade _hasSubjectFacadeSUT;

  public HasSubjectFacadeTests(ITestOutputHelper output) : base(output) {
    _hasSubjectFacadeSUT = new HasSubjectFacade(UnitOfWorkFactory, HasSubjectModelMapper);
  }

  [Fact]
  public async Task Create_WithNonExistingSubject_Throw() {
    // Arrange
    var model = HasSubjectDetailModel.Empty;

    // Act & Assert
    await Assert.ThrowsAnyAsync<InvalidOperationException>(() =>
      _hasSubjectFacadeSUT.SaveAsync(model, Guid.Empty));
  }

  [Fact]
  public async Task Create_WithExistingSubject_Created() {
    // Arrange
    var model = new HasSubjectDetailModel {
      SubjectId = SubjectSeeds.SubjectNoRefs.Id,
      SubjectName = SubjectSeeds.SubjectNoRefs.Name,
      SubjectShortcut = SubjectSeeds.SubjectNoRefs.Shortcut,
      UserId = UserSeeds.User.Id,
      UserLogin = UserSeeds.User.Login,
      UserFirstName = UserSeeds.User.FirstName,
      UserLastName = UserSeeds.User.LastName,
      UserType = UserSeeds.User.UserType,
    };

    // Act
    await _hasSubjectFacadeSUT.SaveAsync(model, UserSeeds.User.Id);

    // Assert
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
    Assert.True(await dbxAssert.SubjectEntities.AnyAsync(i =>
      i.Users.Any(enrollment =>
        enrollment.SubjectId == i.Id && enrollment.UserId == UserSeeds.User.Id)));
  }

  [Fact]
  public async Task Delete_HasSubjectWithEvaluations_DoesNotThrow() {
    //Act & Assert
    await _hasSubjectFacadeSUT.DeleteAsync(HasSubjectSeeds.HasSubject.Id);
  }

  [Fact]
  public async Task SeededHasSubject_DeleteById_Deleted() {
    // Arrange
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    // Act
    await _hasSubjectFacadeSUT.DeleteAsync(HasSubjectSeeds.HasSubjectDelete.Id);

    // Assert
    Assert.False(
      await dbxAssert.HasSubjectEntities.AnyAsync(i =>
        i.Id == HasSubjectSeeds.HasSubjectDelete.Id));
  }

  [Fact]
  public async Task Delete_HasSubjectWithEvaluations_Cascade_Deleted() {
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    await _hasSubjectFacadeSUT.DeleteAsync(HasSubjectSeeds.HasSubject.Id);

    Assert.False(
      await dbxAssert.HasSubjectEntities.AnyAsync(i => i.Id == HasSubjectSeeds.HasSubject.Id));
    Assert.False(
      await dbxAssert.EvaluationEntities.AnyAsync(e =>
        e.StudentId == HasSubjectSeeds.HasSubject.UserId &&
        e.ActivityId == ActivitySeeds.Activity.Id));
    Assert.True(
      await dbxAssert.EvaluationEntities.AnyAsync(e =>
        e.StudentId == HasSubjectSeeds.HasSubject.UserId &&
        e.ActivityId == ActivitySeeds.Activity3.Id));
  }
}