using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Mappers;

public class SubjectEntityMapper : IEntityMapper<SubjectEntity> {
  public void MapToExistingEntity(SubjectEntity existingEntity, SubjectEntity newEntity) {
    existingEntity.Name = newEntity.Name;
    existingEntity.Info = newEntity.Info;
    existingEntity.Shortcut = newEntity.Shortcut;
  }
}