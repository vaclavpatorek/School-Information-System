using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers.Interfaces;

public interface
  IActivityModelMapper : IModelMapper<ActivityEntity, ActivityListModel, ActivityDetailModel> {
  ActivityListModel MapToListModel(ActivityDetailModel model);
}