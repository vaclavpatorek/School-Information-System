using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Mappers.Interfaces;

namespace SchoolIS.BL.Facades;

public class ActivityFacade(
  IUnitOfWorkFactory unitOfWorkFactory,
  IActivityModelMapper activityModelMapper) :
  FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>(
    unitOfWorkFactory, activityModelMapper),
  IActivityFacade {
  protected override ICollection<string> IncludesNavigationPathDetail =>
    new[] {
      $"{nameof(ActivityEntity.Evaluations)}.{nameof(EvaluationEntity.Student)}",
      $"{nameof(ActivityEntity.Subject)}",
      $"{nameof(ActivityEntity.Creator)}",
      $"{nameof(ActivityEntity.Room)}",
    };

  public async Task<IEnumerable<ActivityListModel>> FilterActivitiesByDateAndUser(Guid? userId,
    DateTime? startDate = null, DateTime? endDate = null) {
    return await FilterByDate(startDate, endDate, userId != null
      ? e => e.UserId == userId
      : null);
  }

  public async Task<IEnumerable<ActivityListModel>> FilterActivitiesByDateAndSubject(
    Guid? subjectId, DateTime? startDate = null, DateTime? endDate = null) {
    return await FilterByDate(startDate, endDate, subjectId != null
      ? e => e.SubjectId == subjectId
      : null);
  }

  private async Task<IEnumerable<ActivityListModel>> FilterByDate(
    DateTime? startDate, DateTime? endDate, Func<HasSubjectEntity, bool>? filter) {
    await using var uow = UnitOfWorkFactory.Create();
    var query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

    if (startDate != null) {
      query = query.Where(a => a.StartsFrom >= startDate);
    }

    if (endDate != null) {
      query = query.Where(a => a.EndsAt <= endDate.Value.AddDays(1));
    }

    if (filter != null) {
      // Find all subjects of the provided user
      var subjects = uow.GetRepository<HasSubjectEntity, HasSubjectEntityMapper>().Get()
        .AsEnumerable()
        .Where(filter);

      // Retrieve all activities associated with the same subject as the provided user.
      var subjectIds = subjects.Select(e => e.SubjectId);
      query = query.Where(a => subjectIds.Contains(a.SubjectId));
    }

    foreach (var path in IncludesNavigationPathDetail) {
      query = query.Include(path);
    }

    var activities = await query.ToListAsync().ConfigureAwait(false);

    return activities.Select(activityModelMapper.MapToListModel)
      .OrderByDescending(e => e.StartsFrom);
  }
}