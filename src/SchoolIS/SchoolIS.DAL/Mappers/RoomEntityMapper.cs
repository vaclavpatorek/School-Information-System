using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Mappers;

public class RoomEntityMapper : IEntityMapper<RoomEntity> {
  public void MapToExistingEntity(RoomEntity existingEntity, RoomEntity newEntity) {
    existingEntity.Name = newEntity.Name;
  }
}