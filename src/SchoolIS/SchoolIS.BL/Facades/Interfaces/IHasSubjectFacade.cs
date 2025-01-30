using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Facades.Interfaces;

public interface IHasSubjectFacade 
  : IFacade<HasSubjectEntity, HasSubjectListModel, HasSubjectDetailModel> {
  Task<HasSubjectDetailModel> SaveAsync(HasSubjectDetailModel model, Guid userId);
  Task<IEnumerable<HasSubjectListModel>> GetSubjectStudentsAsync(Guid id);
}