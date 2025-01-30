using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.Repositories;
using SchoolIS.DAL.UnitOfWork;

namespace SchoolIS.BL.Facades;

public abstract class
  FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper>(
    IUnitOfWorkFactory unitOfWorkFactory,
    IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
  : IFacade<TEntity, TListModel, TDetailModel>
  where TEntity : class, IEntity
  where TListModel : IModel
  where TDetailModel : class, IModel
  where TEntityMapper : IEntityMapper<TEntity>, new() {
  protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper = modelMapper;
  protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

  protected virtual ICollection<string> IncludesNavigationPathDetail => new List<string>();

  public virtual async Task DeleteAsync(Guid id) {
    await using IUnitOfWork uow = UnitOfWorkFactory.Create();
    try {
      await uow.GetRepository<TEntity, TEntityMapper>().DeleteAsync(id).ConfigureAwait(false);
      await uow.CommitAsync().ConfigureAwait(false);
    } catch (DbUpdateException e) {
      throw new InvalidOperationException("Entity deletion failed.", e);
    }
  }

  public virtual async Task<TDetailModel?> GetAsync(Guid id) {
    await using IUnitOfWork uow = UnitOfWorkFactory.Create();

    IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

    foreach (string includePath in IncludesNavigationPathDetail) {
      query = query.Include(includePath);
    }

    TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

    return entity is null
      ? null
      : ModelMapper.MapToDetailModel(entity);
  }

  public async Task<IEnumerable<TListModel>> GetAsync(string? orderBy = null, bool desc = false) {
    await using IUnitOfWork uow = UnitOfWorkFactory.Create();

    IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

    foreach (string includePath in IncludesNavigationPathDetail) {
      query = query.Include(includePath);
    }

    List<TEntity> entities = await query.ToListAsync().ConfigureAwait(false);
    IEnumerable<TListModel> listModels = ModelMapper.MapToListModel(entities);

    // No specific order
    if (string.IsNullOrEmpty(orderBy)) {
      return listModels;
    }

    // Invalid property name
    if (query.PropertyExists(orderBy) == false) {
      throw new ArgumentException(
        $"Property {orderBy} does not exist in type {typeof(TEntity).Name}");
    }

    // Ordered by property 
    return desc
      ? listModels.OrderByPropertyDescending(orderBy)
      : listModels.OrderByProperty(orderBy);
  }


  public virtual async Task<TDetailModel> SaveAsync(TDetailModel model) {
    GuardCollectionsAreNotSet(model);

    TEntity entity = ModelMapper.MapToEntity(model);

    IUnitOfWork uow = UnitOfWorkFactory.Create();
    IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

    try {
      if (await repository.ExistsAsync(entity).ConfigureAwait(false)) {
        entity = await repository.UpdateAsync(entity).ConfigureAwait(false);
      } else {
        entity.Id = Guid.NewGuid();
        repository.Insert(entity);
      }

      await uow.CommitAsync().ConfigureAwait(false);
    } catch (DbUpdateException e) {
      throw new InvalidOperationException("Error during database operation: " + e.Message, e);
    }

    return (await GetAsync(entity.Id))!;
  }

  private static void GuardCollectionsAreNotSet(TDetailModel model) {
    IEnumerable<PropertyInfo> collectionProperties = model
      .GetType()
      .GetProperties()
      .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

    foreach (PropertyInfo collectionProperty in collectionProperties) {
      if (collectionProperty.GetValue(model) is ICollection { Count: > 0 }) {
        throw new InvalidOperationException(
          "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
      }
    }
  }
}