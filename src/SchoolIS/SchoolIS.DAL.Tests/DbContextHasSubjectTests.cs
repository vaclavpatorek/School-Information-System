using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using SchoolIS.DAL.Entities;
using Xunit.Abstractions;

namespace SchoolIS.DAL.Tests;

public class DbContextHasSubjectTests(ITestOutputHelper output) : DbContextTestsBase(output) {
  [Fact]
  public async Task AddNew_HasSubject_Persisted() {
    //Arrange
    HasSubjectEntity entity = new() {
      Id = Guid.Parse("a27bd3ae-3ed4-4e77-9e41-33b049887076"),
      UserId = UserSeeds.User.Id,
      SubjectId = SubjectSeeds.Subject.Id,
      Subject = null!,
      User = null!
    };

    //Act
    SchoolISDbContextSUT.HasSubjectEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    HasSubjectEntity actualEntities = await dbx.HasSubjectEntities.SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task AddNew_HasSubjectWithSubjectAndUser_Persisted() {
    //Arrange
    HasSubjectEntity entity = new() {
      Id = Guid.Parse("a27bd3ae-3ed4-4e77-9e41-33b049887076"),
      UserId = UserSeeds.User.Id,
      SubjectId = SubjectSeeds.Subject.Id,
      Subject = new SubjectEntity {
        Id = Guid.Parse("7586cd33-072d-44e1-92c5-ed52736e64fe"),
        Name = "TEST SUBJECT",
        Shortcut = "TS",
        Info = "TEST INFO"
      },
      User = new UserEntity {
        Id = Guid.Parse("e076aed9-fed0-4732-b228-09ad0fd4df91"),
        FirstName = "TEST",
        LastName = "USER",
        PhotoUrl = "TEST_URL.jpg",
        Login = "xTest99",
        PhoneNumber = "0987654321",
        UserType = UserType.Student
      }
    };

    //Act
    SchoolISDbContextSUT.HasSubjectEntities.Add(entity);
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    await using SchoolISDbContext dbx = await DbContextFactory.CreateDbContextAsync();
    HasSubjectEntity actualEntities = await dbx.HasSubjectEntities
      .Include(i => i.Subject)
      .Include(i => i.User)
      .SingleAsync(i => i.Id == entity.Id);
    DeepAssert.Equal(entity, actualEntities);
  }

  [Fact]
  public async Task GetAll_HasSubject_ForUser() {
    //Act
    HasSubjectEntity[] ingredientAmounts = await SchoolISDbContextSUT.HasSubjectEntities
      .Where(i => i.UserId == UserSeeds.User.Id)
      .ToArrayAsync();

    //Assert
    Assert.Contains(HasSubjectSeeds.HasSubject with { User = null!, Subject = null! },
      ingredientAmounts);
    Assert.Contains(HasSubjectSeeds.HasSubjectDelete with { User = null!, Subject = null! },
      ingredientAmounts);
  }

  [Fact]
  public async Task GetAll_HasSubject_ForUserIncludingSubject() {
    //Act
    HasSubjectEntity[] ingredientAmounts = await SchoolISDbContextSUT.HasSubjectEntities
      .Where(i => i.UserId == UserSeeds.User.Id)
      .Include(i => i.Subject)
      .Include(i => i.User)
      .ToArrayAsync();

    //Assert
    Assert.Contains(HasSubjectSeeds.HasSubject.SubjectId, ingredientAmounts.Select(i => i.SubjectId));
    Assert.Contains(HasSubjectSeeds.HasSubjectDelete.SubjectId, ingredientAmounts.Select(i => i.SubjectId));
  }

  [Fact]
  public async Task Delete_HasSubject_Deleted() {
    //Arrange
    HasSubjectEntity baseEntity = HasSubjectSeeds.HasSubjectDelete;

    //Act
    SchoolISDbContextSUT.HasSubjectEntities.Remove(baseEntity with {User = null!, Subject = null!});
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.HasSubjectEntities.AnyAsync(i => i.Id == baseEntity.Id));
  }

  [Fact]
  public async Task DeleteById_HasSubject_HasSubjectDeleted() {
    //Arrange
    HasSubjectEntity entityBase = HasSubjectSeeds.HasSubject;

    //Act
    SchoolISDbContextSUT.Remove(
      SchoolISDbContextSUT.HasSubjectEntities.Single(i => i.Id == entityBase.Id));
    await SchoolISDbContextSUT.SaveChangesAsync();

    //Assert
    Assert.False(await SchoolISDbContextSUT.HasSubjectEntities.AnyAsync(i => i.Id == entityBase.Id));
  }
}