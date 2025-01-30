using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers;

public class HasSubjectModelMapper :
  ModelMapperBase<HasSubjectEntity, HasSubjectListModel, HasSubjectDetailModel>,
  IHasSubjectModelMapper {
  public override HasSubjectListModel MapToListModel(HasSubjectEntity? entity) =>
    ((entity?.Subject is null) || (entity?.User is null))
      ? HasSubjectListModel.Empty
      : new HasSubjectListModel {
        Id = entity.Id,
        UserId = entity.UserId,
        SubjectId = entity.SubjectId,
        SubjectName = entity.Subject.Name,
        SubjectShortcut = entity.Subject.Shortcut,
        UserFirstName = entity.User.FirstName,
        UserLastName = entity.User.LastName,
        UserType = entity.User.UserType,
      };

  public override HasSubjectDetailModel MapToDetailModel(HasSubjectEntity? entity) =>
    entity?.Subject is null
      ? HasSubjectDetailModel.Empty
      : new HasSubjectDetailModel {
        Id = entity.Id,
        UserId = entity.UserId,
        SubjectId = entity.SubjectId,
        SubjectName = entity.Subject.Name,
        SubjectShortcut = entity.Subject.Shortcut,
        UserLogin = entity.User.Login,
        UserFirstName = entity.User.FirstName,
        UserLastName = entity.User.LastName,
        UserType = entity.User.UserType,
        TotalPoints = (int)entity.User.Evaluations.Where(e => e.Activity.SubjectId == entity.SubjectId).Sum(e => e.Points ?? 0),
      };

  public override HasSubjectEntity MapToEntity(HasSubjectDetailModel model) =>
    throw new NotSupportedException("This method is unsupported. Use the other overload.");

  public HasSubjectEntity MapToEntity(HasSubjectDetailModel model, Guid teacherId) =>
    new() {
      Id = model.Id,
      Subject = null!,
      SubjectId = model.SubjectId,
      User = null!,
      UserId = teacherId
    };

  public HasSubjectEntity MapToEntity(HasSubjectListModel model, Guid teacherId) =>
    new() {
      Id = model.Id,
      Subject = null!,
      SubjectId = model.SubjectId,
      User = null!,
      UserId = teacherId
    };
}