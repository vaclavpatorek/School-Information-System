using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Mappers;

public class HasSubjectEntityMapper : IEntityMapper<HasSubjectEntity> {
  public void MapToExistingEntity(HasSubjectEntity existingEntity, HasSubjectEntity newEntity) {
    existingEntity.SubjectId = newEntity.SubjectId;
    existingEntity.UserId = newEntity.UserId;
  }
}