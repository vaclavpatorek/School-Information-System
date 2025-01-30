using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.Repositories;

namespace SchoolIS.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable {
  IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
    where TEntity : class, IEntity
    where TEntityMapper : IEntityMapper<TEntity>, new();

  Task CommitAsync();
}