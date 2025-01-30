using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers;

public class RoomModelMapper :
  ModelMapperBase<RoomEntity, RoomListModel, RoomDetailModel>, IRoomModelMapper {
  public override RoomListModel MapToListModel(RoomEntity? entity) =>
    entity is null
      ? RoomListModel.Empty
      : new() {
        Id = entity.Id,
        Name = entity.Name
      };

  public override RoomDetailModel MapToDetailModel(RoomEntity? entity) =>
    entity is null
      ? RoomDetailModel.Empty
      : new() {
        Id = entity.Id,
        Name = entity.Name
      };

  public override RoomEntity MapToEntity(RoomDetailModel model) =>
    new() {
      Id = model.Id,
      Name = model.Name,
    };
}