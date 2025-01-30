using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.UnitOfWork;

namespace SchoolIS.BL.Facades;

public class SubjectFacade(
  IUnitOfWorkFactory unitOfWorkFactory,
  ISubjectModelMapper subjectModelMapper) :
  FacadeBase<SubjectEntity, SubjectListModel, SubjectDetailModel, SubjectEntityMapper>(
    unitOfWorkFactory, subjectModelMapper), ISubjectFacade {
  protected override ICollection<string> IncludesNavigationPathDetail =>
    new[] {
      $"{nameof(SubjectEntity.Users)}.{nameof(HasSubjectEntity.User)}",
    };
}