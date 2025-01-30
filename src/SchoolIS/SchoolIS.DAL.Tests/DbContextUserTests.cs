using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using SchoolIS.DAL.Entities;
using Xunit.Abstractions;

namespace SchoolIS.DAL.Tests;

public class DbContextUserTests(ITestOutputHelper output) : DbContextTestsBase(output) {
  [Fact]
  public async Task AddNew_User_Persisted() {
    //Arrange
    UserEntity entity = new() {
      Id = Guid.Parse("33b3902d-7d4f-4213-9cf0-112348f56238"),
      FirstName = "TEST",
      LastName = "USER",
      PhotoUrl = "TEST_URL.jpg",
      Login = "xTEST99"
    };

    //Act
    SchoolISDbContextSUT.UserEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    UserEntity actualEntities = await dbx.UserEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task AddNew_UserWithEnrolledSubject_Persisted() {
    //Arrange
    UserEntity entity = new() {
      Id = Guid.Parse("03a39024-7d4f-4213-9cf0-112348f56238"),
      FirstName = "TEST",
      LastName = "USER",
      PhotoUrl = "TEST_URL.jpg",
      Login = "xTEST00",
      UserType = UserType.Teacher,
      Subjects = new List<HasSubjectEntity> {
        new() {
          Id = Guid.Parse("03aa902d-7d4f-4213-9cf0-112348f56238"),
          Subject = new SubjectEntity {
            Id = Guid.Parse("0013902d-7d4f-4213-9cf0-112348f56238"),
            Name = "TEST SUBJECT",
            Shortcut = "TS",
            Info = null
          },
          SubjectId = SubjectSeeds.SubjectNoRefs.Id,
          User = null!,
          UserId = UserSeeds.UserNoRefs.Id
        }
      }
    };

    //Act
    SchoolISDbContextSUT.UserEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    UserEntity actualEntities = await dbx.UserEntities
      .Include(i => i.Subjects)
      .ThenInclude(i => i.Subject)
      .SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task GetAll_User_ContainsUserWithoutReferences() {
    //Act
    UserEntity[] entities = await SchoolISDbContextSUT.UserEntities
      .ToArrayAsync();

    //Assert
    DeepAssert.Contains(UserSeeds.UserNoRefs, entities);
  }


  [Fact]
  public async Task GetById_User_TestUserRetrieved() {
    //Act
    UserEntity entity =
      await SchoolISDbContextSUT.UserEntities
        //.Include(i => i.Activities)
        .SingleAsync(i => i.Id == UserSeeds.UserNoRefs.Id);


    //Assert
    DeepAssert.Equal(UserSeeds.UserNoRefs, entity);
  }

  [Fact]
  public async Task Update_User_Persisted() {
    //Arrange
    UserEntity baseEntity = UserSeeds.UserNoRefs;
    UserEntity entity =
      baseEntity with {
        FirstName = baseEntity + "Updated",
        LastName = baseEntity + "Updated"
      };

    //Act
    SchoolISDbContextSUT.UserEntities.Update(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    UserEntity actualEntity = await dbx.UserEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntity);
  }

  [Fact]
  public async Task Delete_User_UserDeleted() {
    //Arrange
    UserEntity entityBase = UserSeeds.UserNoRefs;

    //Act
    SchoolISDbContextSUT.UserEntities.Remove(entityBase);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.UserEntities.AnyAsync(i => i.Id == entityBase.Id));
  }

  [Fact]
  public async Task DeleteById_User_UserDeleted() {
    //Arrange
    UserEntity entityBase = UserSeeds.User;

    //Act
    SchoolISDbContextSUT.Remove(
      SchoolISDbContextSUT.UserEntities.Single(i => i.Id == entityBase.Id));
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.UserEntities.AnyAsync(i => i.Id == entityBase.Id));
    Assert.False(
      await SchoolISDbContextSUT.EvaluationEntities.AnyAsync(i =>
        i.Id == EvaluationSeeds.Evaluation.Id));
    Assert.False(
      await SchoolISDbContextSUT.HasSubjectEntities.AnyAsync(i => i.Id == HasSubjectSeeds.HasSubject.Id));
    Assert.False(
      await SchoolISDbContextSUT.HasSubjectEntities.AnyAsync(i => i.Id == HasSubjectSeeds.HasSubjectDelete.Id));
    Assert.False(
      await SchoolISDbContextSUT.ActivityEntities.AnyAsync(i =>
        i.Id == ActivitySeeds.Activity.Id));
    Assert.False(
      await SchoolISDbContextSUT.ActivityEntities.AnyAsync(
        i => i.Id == ActivitySeeds.Activity2.Id));
  }
}