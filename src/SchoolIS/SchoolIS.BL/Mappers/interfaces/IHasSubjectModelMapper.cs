using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers.Interfaces;

public interface
  IHasSubjectModelMapper : IModelMapper<HasSubjectEntity, HasSubjectListModel,
  HasSubjectDetailModel> {
  HasSubjectEntity MapToEntity(HasSubjectDetailModel model, Guid teacherId);
}