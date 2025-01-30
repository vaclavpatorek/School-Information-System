using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Facades.Interfaces;

public interface IFacade<TEntity, TListModel, TDetailModel>
  where TEntity : class, IEntity
  where TListModel : IModel
  where TDetailModel : class, IModel {
  Task DeleteAsync(Guid id);
  Task<TDetailModel?> GetAsync(Guid id);
  Task<IEnumerable<TListModel>> GetAsync(string? orderBy = null, bool desc = false);
  Task<TDetailModel> SaveAsync(TDetailModel model);
}