using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Facades.Interfaces;

public interface
  IEvaluationFacade : IFacade<EvaluationEntity, EvaluationListModel, EvaluationDetailModel> {
}