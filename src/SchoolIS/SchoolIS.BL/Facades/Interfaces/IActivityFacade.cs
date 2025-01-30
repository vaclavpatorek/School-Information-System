using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Facades.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel> {
  Task<IEnumerable<ActivityListModel>> FilterActivitiesByDateAndUser(Guid? userId,
    DateTime? startDate = null, DateTime? endDate = null);

  Task<IEnumerable<ActivityListModel>> FilterActivitiesByDateAndSubject(Guid? subjectId,
    DateTime? startDate = null, DateTime? endDate = null);
}