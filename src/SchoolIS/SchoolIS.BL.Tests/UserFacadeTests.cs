using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Enums;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace SchoolIS.BL.Tests;

public sealed class UserFacadeTests : FacadeTestsBase {
  private readonly IUserFacade _userFacadeSUT;

  public UserFacadeTests(ITestOutputHelper output) : base(output) {
    _userFacadeSUT = new UserFacade(UnitOfWorkFactory, UserModelMapper);
  }


  [Fact]
  public async Task Create_WithoutReferences_DoesNotThrow() {
    // Arrange
    var model = UserDetailModel.Empty;

    // Act & Assert
    await _userFacadeSUT.SaveAsync(model);
  }

  [Fact]
  public async Task Create_WithoutReferences_EqualsCreated() {
    // Arrange
    var model = new UserDetailModel {
      Id = Guid.Empty,
      Email = "test@email.com",
      FirstName = "FIRST",
      LastName = "LAST",
      Login = "xcreate",
      PhoneNumber = "1234567890",
      Type = UserType.Student,
    };

    // Act
    UserDetailModel createdModel = await _userFacadeSUT.SaveAsync(model);
    model.Id = createdModel.Id;

    // Assert
    DeepAssert.Equal(model, createdModel);
  }

  [Fact]
  public async Task Create_WithNonExistingEnrollment_Throws() {
    // Arrange
    Guid userId = Guid.Parse("74340468-7FA6-4BB8-8C76-2AE39599BB2E");

    var model = new UserDetailModel {
      Id = userId,
      Email = "test@email.com",
      FirstName = "FIRST",
      LastName = "LAST",
      Login = "xcreate",
      PhoneNumber = "1234567890",
      Type = UserType.Student,
      Subjects = [
        new HasSubjectListModel {
          SubjectId = SubjectSeeds.SubjectNoRefs.Id,
          SubjectName = SubjectSeeds.SubjectNoRefs.Name,
          SubjectShortcut = SubjectSeeds.SubjectNoRefs.Shortcut,
          UserId = userId,
          UserFirstName = "FIRST",
          UserLastName = "LAST",
          UserType = UserType.Student,
        }
      ]
    };

    // Act & Assert
    await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _userFacadeSUT.SaveAsync(model));
  }

  [Fact]
  public async Task GetAll_Single_SeededUser() {
    // Act 
    var users = await _userFacadeSUT.GetAsync();
    var user = users.Single(i => i.Id == UserSeeds.User.Id);

    // Assert
    DeepAssert.Equal(UserModelMapper.MapToListModel(UserSeeds.User), user);
  }


  [Fact]
  public async Task GetAll_SeededUser_Ascending() {
    // Act 
    var users = await _userFacadeSUT.GetAsync(orderBy: "Login", desc: false);

    // Assert
    IEnumerable<UserListModel> mapToListModel = UserModelMapper.MapToListModel([
      UserSeeds.UserDelete, UserSeeds.User, UserSeeds.UserNoRefs, UserSeeds.User2
    ]);

    DeepAssert.Equal(mapToListModel, users);
  }

  [Fact]
  public async Task GetById_SeededUser() {
    // Act 
    var user = await _userFacadeSUT.GetAsync(UserSeeds.User.Id);

    // Assert
    DeepAssert.Equal(UserModelMapper.MapToDetailModel(UserSeeds.User), user);
  }

  [Fact]
  public async Task GetById_NonExistent() {
    // Act 
    var user = await _userFacadeSUT.GetAsync(UserSeeds.EmptyUser.Id);

    // Assert
    Assert.Null(user);
  }

  [Fact]
  public async Task SeededUser_DeleteById_Deleted() {
    // Arrange
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    // Act 
    await _userFacadeSUT.DeleteAsync(UserSeeds.UserDelete.Id);

    // Assert
    Assert.False(await dbxAssert.UserEntities.AnyAsync(i => i.Id == UserSeeds.UserDelete.Id));
  }

  [Fact]
  public async Task Delete_UserUsedByOtherEntities_DoesNotThrow() {
    //Act & Assert
    await _userFacadeSUT.DeleteAsync(UserSeeds.User.Id); // Cascade deletion of all related entities
  }

  [Fact]
  public async Task NewUser_InsertOrUpdate_UserAdded() {
    //Arrange
    var user = new UserDetailModel() {
      Id = Guid.Empty,
      Email = "test@email.com",
      FirstName = "FIRST",
      LastName = "LAST",
      Login = "updated",
      Type = UserType.Student,
    };
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    //Act
    var createdUser = await _userFacadeSUT.SaveAsync(user);
    user.Id = createdUser.Id;

    //Assert
    var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
    DeepAssert.Equal(user, UserModelMapper.MapToDetailModel(userFromDb));
  }

  [Fact]
  public async Task SeededUser_InsertOrUpdate_UserUpdated() {
    //Arrange
    var user = new UserDetailModel() {
      Id = UserSeeds.User.Id,
      FirstName = UserSeeds.User.FirstName,
      LastName = UserSeeds.User.LastName,
      Login = UserSeeds.User.Login,
      Type = UserSeeds.User.UserType,
    };

    user.FirstName += "updated";

    //Act
    await _userFacadeSUT.SaveAsync(user);

    //Assert
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
    var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
    DeepAssert.Equal(user, UserModelMapper.MapToDetailModel(userFromDb));
  }
}