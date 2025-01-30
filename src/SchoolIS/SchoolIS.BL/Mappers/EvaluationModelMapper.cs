using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers;

public class EvaluationModelMapper :
  ModelMapperBase<EvaluationEntity, EvaluationListModel, EvaluationDetailModel>,
  IEvaluationModelMapper {
  public override EvaluationListModel MapToListModel(EvaluationEntity? entity) =>
    entity is null
      ? EvaluationListModel.Empty
      : new() {
        Id = entity.Id,
        ActivityId = entity.ActivityId,
        StudentId = entity.StudentId,
        StudentLogin = entity.Student.Login,
        StudentFirstName = entity.Student.FirstName,
        StudentLastName = entity.Student.LastName,
        Note = entity.Note,
        Points = entity.Points ?? 0,
        Attended = entity.Points is not null,
      };

  public override EvaluationDetailModel MapToDetailModel(EvaluationEntity? entity) =>
    entity is null
      ? EvaluationDetailModel.Empty
      : new() {
        Id = entity.Id,
        StudentId = entity.StudentId,
        StudentLogin = entity.Student.Login,
        StudentFirstName = entity.Student.FirstName,
        StudentLastName = entity.Student.LastName,
        Note = entity.Note,
        Points = entity.Points ?? 0,
        Attended = entity.Points is not null,
        ActivityId = entity.ActivityId,
      };

  public override EvaluationEntity MapToEntity(EvaluationDetailModel model) =>
    new() {
      Id = model.Id,
      Activity = null!,
      ActivityId = model.ActivityId,
      Note = model.Note,
      Points = model.Attended ? model.Points : null,
      Student = null!,
      StudentId = model.StudentId
    };

  public EvaluationListModel MapToListModel(EvaluationDetailModel model) =>
    new() {
      ActivityId = model.ActivityId,
      Attended = model.Attended,
      Id = model.Id,
      Note = model.Note,
      Points = model.Points,
      StudentFirstName = model.StudentFirstName,
      StudentLastName = model.StudentLastName,
      StudentId = model.StudentId,
      StudentLogin = model.StudentLogin
    };
}