using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers;

public class SubjectModelMapper(IHasSubjectModelMapper hasSubjectModelMapper) :
  ModelMapperBase<SubjectEntity, SubjectListModel, SubjectDetailModel>, ISubjectModelMapper {
  public override SubjectListModel MapToListModel(SubjectEntity? entity) {
    if (entity is null) 
      return SubjectListModel.Empty;

    return new SubjectListModel {
      Id = entity.Id,
      Name = entity.Name,
      Shortcut = entity.Shortcut,
      StudentCount = (uint)entity.Users.Count(e => e.User.UserType == UserType.Student),
    };
  }

  public override SubjectDetailModel MapToDetailModel(SubjectEntity? entity) {
    if (entity is null) return SubjectDetailModel.Empty;

    return new SubjectDetailModel {
      Id = entity.Id,
      Shortcut = entity.Shortcut,
      Name = entity.Name,
      Info = entity.Info,
      Students = entity.Users
        .Where(e => e.User.UserType == UserType.Student)
        .Select(hasSubjectModelMapper.MapToListModel)
        .ToObservableCollection(),
      Teachers = entity.Users
        .Where(e => e.User.UserType == UserType.Teacher)
        .Select(hasSubjectModelMapper.MapToListModel)
        .ToObservableCollection(),
    };
  }

  public override SubjectEntity MapToEntity(SubjectDetailModel model) =>
    new() {
      Id = model.Id,
      Name = model.Name,
      Shortcut = model.Shortcut,
      Info = model.Info,
    };
}