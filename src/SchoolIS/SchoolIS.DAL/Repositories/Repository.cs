using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;

namespace SchoolIS.DAL.Repositories;

public class Repository<TEntity>(DbContext dbContext, IEntityMapper<TEntity> entityMapper)
  : IRepository<TEntity>
  where TEntity : class, IEntity {
  private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

  public IQueryable<TEntity> Get() {
    return _dbSet;
  }

  public async Task DeleteAsync(Guid entityId)
    => _dbSet.Remove(await _dbSet.SingleAsync(i => i.Id == entityId).ConfigureAwait(false));

  public async ValueTask<bool> ExistsAsync(TEntity entity) {
    return entity.Id != Guid.Empty && await _dbSet.AnyAsync(e => e.Id == entity.Id);
  }

  public TEntity Insert(TEntity entity) => _dbSet.Add(entity).Entity;

  public async Task<TEntity> UpdateAsync(TEntity entity) {
    TEntity existingEntity = await _dbSet.SingleAsync(e => e.Id == entity.Id);
    entityMapper.MapToExistingEntity(existingEntity, entity);
    return existingEntity;
  }
}