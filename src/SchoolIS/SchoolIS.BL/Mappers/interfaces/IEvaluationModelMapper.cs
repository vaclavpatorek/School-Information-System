using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers.Interfaces;

public interface IEvaluationModelMapper
  : IModelMapper<EvaluationEntity, EvaluationListModel, EvaluationDetailModel> {
  public EvaluationListModel MapToListModel(EvaluationDetailModel model);
}