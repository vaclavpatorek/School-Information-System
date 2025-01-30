using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity> {
  public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity) {
    existingEntity.ActivityType = newEntity.ActivityType;
    existingEntity.CreatorId = newEntity.CreatorId;
    existingEntity.StartsFrom = newEntity.StartsFrom;
    existingEntity.EndsAt = newEntity.EndsAt;
    existingEntity.RoomId = newEntity.RoomId;
  }
}