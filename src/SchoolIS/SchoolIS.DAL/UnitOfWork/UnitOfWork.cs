using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.Repositories;

namespace SchoolIS.DAL.UnitOfWork;

public sealed class UnitOfWork(DbContext dbContext) : IUnitOfWork {
  private readonly DbContext _dbContext =
    dbContext ?? throw new ArgumentNullException(nameof(dbContext));

  public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
    where TEntity : class, IEntity
    where TEntityMapper : IEntityMapper<TEntity>, new() {
    return new Repository<TEntity>(_dbContext, new TEntityMapper());
  }

  public async Task CommitAsync() {
    await _dbContext.SaveChangesAsync();
  }

  public async ValueTask DisposeAsync() {
    await _dbContext.DisposeAsync();
  }
}