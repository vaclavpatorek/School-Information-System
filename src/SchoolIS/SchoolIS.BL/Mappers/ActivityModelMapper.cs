using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers;

public class ActivityModelMapper(IEvaluationModelMapper evaluationModelMapper) :
  ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>, IActivityModelMapper {
  public override ActivityListModel MapToListModel(ActivityEntity? entity) =>
    entity is null
      ? ActivityListModel.Empty
      : new() {
        Id = entity.Id,
        RoomId = entity.RoomId,
        CreatorId = entity.CreatorId,
        SubjectId = entity.SubjectId,
        StartsFrom = entity.StartsFrom,
        EndsAt = entity.EndsAt,
        ActivityType = entity.ActivityType,
        CreatorFirstName = entity.Creator.FirstName,
        CreatorLastName = entity.Creator.LastName,
        RoomName = entity.Room.Name
      };

  public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity) =>
    entity is null
      ? ActivityDetailModel.Empty
      : new() {
        Id = entity.Id,
        RoomId = entity.RoomId,
        CreatorId = entity.CreatorId,
        SubjectId = entity.SubjectId,
        StartsFrom = entity.StartsFrom,
        EndsAt = entity.EndsAt,
        ActivityType = entity.ActivityType,
        CreatorFirstName = entity.Creator.FirstName,
        CreatorLastName = entity.Creator.LastName,
        SubjectName = entity.Subject.Name,
        SubjectShortcut = entity.Subject.Shortcut,
        RoomName = entity.Room.Name,
        Evaluations = evaluationModelMapper.MapToListModel(entity.Evaluations)
          .ToObservableCollection()
      };

  public override ActivityEntity MapToEntity(ActivityDetailModel model) =>
    new() {
      Id = model.Id,
      StartsFrom = model.StartsFrom,
      EndsAt = model.EndsAt,
      ActivityType = model.ActivityType,
      SubjectId = model.SubjectId,
      CreatorId = model.CreatorId,
      RoomId = model.RoomId,
      Subject = null!,
      Creator = null!,
      Room = null!,
    };

  public ActivityListModel MapToListModel(ActivityDetailModel model)
    => new() {
      Id = model.Id,
      ActivityType = model.ActivityType,
      StartsFrom = model.StartsFrom,
      EndsAt = model.EndsAt,
      CreatorFirstName = model.CreatorFirstName,
      CreatorId = model.CreatorId,
      CreatorLastName = model.CreatorLastName,
      RoomId = model.RoomId,
      RoomName = model.RoomName,
      SubjectId = model.SubjectId,
    };
}