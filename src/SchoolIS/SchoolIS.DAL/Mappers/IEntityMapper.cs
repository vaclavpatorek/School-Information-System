using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Mappers;

public interface IEntityMapper<in TEntity>
  where TEntity : IEntity {
  void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
}