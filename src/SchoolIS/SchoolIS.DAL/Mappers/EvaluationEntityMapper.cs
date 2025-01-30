using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Mappers;

public class EvaluationEntityMapper : IEntityMapper<EvaluationEntity> {
  public void MapToExistingEntity(EvaluationEntity existingEntity, EvaluationEntity newEntity) {
    existingEntity.ActivityId = newEntity.ActivityId;
    existingEntity.StudentId = newEntity.StudentId;
    existingEntity.Note = newEntity.Note;
    existingEntity.Points = newEntity.Points;
  }
}