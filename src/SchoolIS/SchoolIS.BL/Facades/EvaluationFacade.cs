using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.UnitOfWork;

namespace SchoolIS.BL.Facades;

public class EvaluationFacade(
  IUnitOfWorkFactory unitOfWorkFactory,
  IEvaluationModelMapper evaluationModelMapper) :
  FacadeBase<EvaluationEntity, EvaluationListModel, EvaluationDetailModel, EvaluationEntityMapper>(
    unitOfWorkFactory,
    evaluationModelMapper), IEvaluationFacade {
  protected override ICollection<string> IncludesNavigationPathDetail =>
    new[] {
      $"{nameof(EvaluationEntity.Student)}",
      $"{nameof(EvaluationEntity.Activity)}",
    };
}