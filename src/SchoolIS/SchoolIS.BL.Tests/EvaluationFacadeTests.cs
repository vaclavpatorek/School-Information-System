using SchoolIS.BL.Facades;
using SchoolIS.BL.Models;
using SchoolIS.Common.Tests;
using Xunit.Abstractions;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;

namespace SchoolIS.BL.Tests;

public class EvaluationFacadeTests : FacadeTestsBase {
  private readonly IEvaluationFacade _evaluationFacadeSUT;

  public EvaluationFacadeTests(ITestOutputHelper output) : base(output) {
    _evaluationFacadeSUT = new EvaluationFacade(UnitOfWorkFactory, EvaluationModelMapper);
  }

  [Fact]
  public async Task Create_WithNonExistingStudent_Throws() {
    //Arrange
    var model = EvaluationDetailModel.Empty;

    //Act & Assert
    await Assert.ThrowsAnyAsync<InvalidOperationException>(() =>
      _evaluationFacadeSUT.SaveAsync(model));
  }

  [Fact]
  public async Task Create_WithExistingStudentAndActivity_EqualsCreated() {
    //Arrange
    var model = new EvaluationDetailModel {
      Attended = true,
      Points = 10,
      Note = "TEST NOTE",
      StudentFirstName = UserSeeds.User.FirstName,
      StudentLastName = UserSeeds.User.LastName,
      StudentLogin = UserSeeds.User.Login,
      StudentId = UserSeeds.User.Id,
      ActivityId = ActivitySeeds.Activity.Id
    };

    //Act
    var returnedModel = await _evaluationFacadeSUT.SaveAsync(model);
    model.Id = returnedModel.Id;

    //Assert
    DeepAssert.Equal(model, returnedModel);
  }


  [Fact]
  public async Task GetById_FromSeeded_EqualsSeeded() {
    //Arrange
    var detailModel = EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.Evaluation);

    //Act
    var returnedModel = await _evaluationFacadeSUT.GetAsync(detailModel.Id);

    //Assert
    DeepAssert.Equal(detailModel, returnedModel);
  }

  [Fact]
  public async Task GetAll_FromSeeded_ContainsSeeded() {
    //Arrange
    var listModel = EvaluationModelMapper.MapToListModel(EvaluationSeeds.Evaluation);

    //Act
    var returnedModel = await _evaluationFacadeSUT.GetAsync();

    //Assert
    Assert.Contains(listModel, returnedModel);
  }

  [Fact]
  public async Task Update_FromSeeded_DoesNotThrow() {
    //Arrange
    var detailModel = EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.Evaluation);
    detailModel.Note = "UPDATED";

    //Act & Assert
    await _evaluationFacadeSUT.SaveAsync(detailModel);
  }

  [Fact]
  public async Task Update_Note_FromSeeded_Updated() {
    //Arrange
    var detailModel = EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.Evaluation);
    detailModel.Note = "UPDATED";

    //Act
    await _evaluationFacadeSUT.SaveAsync(detailModel);

    //Assert
    var returnedModel = await _evaluationFacadeSUT.GetAsync(detailModel.Id);
    DeepAssert.Equal(detailModel, returnedModel);
  }

  [Fact]
  public async Task Update_RenameStudent_FromSeeded_NotUpdated() {
    //Arrange
    var detailModel = EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.Evaluation);
    detailModel.StudentFirstName = "UPDATED";

    //Act
    await _evaluationFacadeSUT.SaveAsync(detailModel);

    //Assert
    var returnedModel = await _evaluationFacadeSUT.GetAsync(detailModel.Id);
    DeepAssert.Equal(EvaluationModelMapper.MapToDetailModel(EvaluationSeeds.Evaluation),
      returnedModel);
  }


  [Fact]
  public async Task DeleteById_FromSeeded_DoesNotThrow() {
    //Arrange & Act & Assert
    await _evaluationFacadeSUT.DeleteAsync(EvaluationSeeds.Evaluation.Id);
  }
  
  [Fact]
  public async Task SeededActivity_DeleteById_Deleted() {
    // Arrange
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    // Act
    await _evaluationFacadeSUT.DeleteAsync(EvaluationSeeds.Evaluation.Id);

    // Assert
    Assert.False(
      await dbxAssert.EvaluationEntities.AnyAsync(i => i.Id == EvaluationSeeds.Evaluation.Id));
  }
}