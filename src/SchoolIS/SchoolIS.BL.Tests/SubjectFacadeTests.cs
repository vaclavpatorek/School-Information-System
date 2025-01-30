using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace SchoolIS.BL.Tests;

public sealed class SubjectFacadeTests : FacadeTestsBase {
  private readonly ISubjectFacade _subjectFacadeSUT;

  public SubjectFacadeTests(ITestOutputHelper output) : base(output) {
    _subjectFacadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper);
  }

  [Fact]
  public async Task Create_WithoutReferences_DoesNotThrow() {
    // Arrange
    var model = SubjectDetailModel.Empty;

    // Act
    await _subjectFacadeSUT.SaveAsync(model);
  }

  [Fact]
  public async Task Create_WithTeachers_Throws() {
    // Arrange
    var model = new SubjectDetailModel {
      Id = Guid.Empty,
      Name = "Test",
      Shortcut = "TST",
      Teachers = [
        new HasSubjectListModel {
          Id = ActivitySeeds.Activity.Id,
          UserId = ActivitySeeds.Activity.CreatorId,
          UserType = ActivitySeeds.Activity.Creator.UserType,
          SubjectId = ActivitySeeds.Activity.SubjectId,
          UserFirstName = UserSeeds.User.FirstName,
          UserLastName = UserSeeds.User.LastName,
          SubjectName = ActivitySeeds.Activity.Subject.Name,
          SubjectShortcut = ActivitySeeds.Activity.Subject.Shortcut,
        }
      ]
    };

    // Act & Assert
    await Assert.ThrowsAnyAsync<InvalidOperationException>(() =>
      _subjectFacadeSUT.SaveAsync(model));
  }


  [Fact]
  public async Task GetAll_Single_SeededSubject() {
    // Act
    var subjects = await _subjectFacadeSUT.GetAsync();
    var subject = subjects.Single(i => i.Id == SubjectSeeds.Subject.Id);

    // Assert
    DeepAssert.Equal(SubjectModelMapper.MapToListModel(SubjectSeeds.Subject), subject);
  }

  [Fact]
  public async Task GetAll_Subjects_Descending() {
    // Act 
    var subjects = await _subjectFacadeSUT.GetAsync(orderBy: "Shortcut", desc: true);

    // Assert
    IEnumerable<SubjectListModel> mapToListModel = SubjectModelMapper.MapToListModel([
      SubjectSeeds.SubjectNoRefs, SubjectSeeds.Subject2, SubjectSeeds.SubjectDelete,
      SubjectSeeds.Subject
    ]);

    DeepAssert.Equal(mapToListModel, subjects);
  }

  [Fact]
  public async Task GetAll_Subjects_InvalidPropertyThrows() {
    // Act & Assert 
    await Assert.ThrowsAnyAsync<ArgumentException>(() =>
      _subjectFacadeSUT.GetAsync(orderBy: "INVALID PROPERTY", desc: true));
  }

  [Fact]
  public async Task GetById_SeededSubject() {
    // Act
    var subject = await _subjectFacadeSUT.GetAsync(SubjectSeeds.Subject.Id);

    // Assert
    SubjectDetailModel mapToDetailModel = SubjectModelMapper.MapToDetailModel(SubjectSeeds.Subject);
    DeepAssert.Equal(mapToDetailModel, subject);
  }

  [Fact]
  public async Task GetById_NonExistent() {
    // Act
    var subject = await _subjectFacadeSUT.GetAsync(SubjectSeeds.EmptySubject.Id);

    // Assert
    Assert.Null(subject);
  }

  [Fact]
  public async Task SeededSubject_DeleteById_Deleted() {
    // Arrange
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    // Act
    await _subjectFacadeSUT.DeleteAsync(SubjectSeeds.SubjectDelete.Id);

    // Assert
    Assert.False(
      await dbxAssert.SubjectEntities.AnyAsync(i => i.Id == SubjectSeeds.SubjectDelete.Id));
  }

  [Fact]
  public async Task Delete_ReferencedSubjectByOtherEntities_DoesNotThrow() {
    //Act & Assert
    await _subjectFacadeSUT.DeleteAsync(SubjectSeeds.Subject.Id);
  }

  [Fact]
  public async Task NewSubject_InsertOrUpdate_SubjectAdded() {
    //Arrange
    var subject = new SubjectDetailModel {
      Id = Guid.Empty,
      Name = "Test",
      Shortcut = "TST",
    };

    //Act
    subject = await _subjectFacadeSUT.SaveAsync(subject);

    //Assert
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
    var subjectFromDb = await dbxAssert.SubjectEntities.SingleAsync(i => i.Id == subject.Id);
    DeepAssert.Equal(subject, SubjectModelMapper.MapToDetailModel(subjectFromDb));
  }

  [Fact]
  public async Task SeededSubject_InsertOrUpdate_SubjectUpdated() {
    //Arrange
    var subject = new SubjectDetailModel {
      Id = SubjectSeeds.Subject.Id,
      Name = SubjectSeeds.Subject.Name,
      Shortcut = SubjectSeeds.Subject.Shortcut,
    };

    subject.Name += "updated";

    //Act
    await _subjectFacadeSUT.SaveAsync(subject);

    //Assert
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
    var subjectFromDb = await dbxAssert.SubjectEntities.SingleAsync(i => i.Id == subject.Id);
    DeepAssert.Equal(subject, SubjectModelMapper.MapToDetailModel(subjectFromDb));
  }
}