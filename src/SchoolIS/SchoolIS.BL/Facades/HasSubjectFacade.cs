using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.Repositories;
using SchoolIS.DAL.UnitOfWork;

namespace SchoolIS.BL.Facades;

public class HasSubjectFacade(
  IUnitOfWorkFactory unitOfWorkFactory,
  IHasSubjectModelMapper hasSubjectModelMapper) :
  FacadeBase<HasSubjectEntity, HasSubjectListModel, HasSubjectDetailModel,
    HasSubjectEntityMapper>(
    unitOfWorkFactory,
    hasSubjectModelMapper), IHasSubjectFacade {
  protected override ICollection<string> IncludesNavigationPathDetail =>
    new[] {
      $"{nameof(HasSubjectEntity.User)}.{nameof(UserEntity.Evaluations)}.{nameof(EvaluationEntity.Activity)}",
      $"{nameof(HasSubjectEntity.Subject)}",
    };

  // FIXME: If we had ternary relation between User, Subject and Evaluation, then this
  //        wouldn't be necessary.
  public override async Task DeleteAsync(Guid id) {
    await using IUnitOfWork uow = UnitOfWorkFactory.Create();
    try {
      HasSubjectEntity? hasSubject = uow.GetRepository<HasSubjectEntity, HasSubjectEntityMapper>()
        .Get().FirstOrDefault(h => h.Id == id);

      if (hasSubject == null)
        throw new InvalidOperationException($"Invalid HasSubjectEntity id: {id}");

      // Delete the HasSubjectEntity
      await uow.GetRepository<HasSubjectEntity, HasSubjectEntityMapper>().DeleteAsync(id)
        .ConfigureAwait(false);

      // Delete all evaluations of this student in this subject.
      var evalRepo = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();
      var evaluations = evalRepo.Get().Where(e =>
        e.StudentId == hasSubject.UserId && e.Activity.SubjectId == hasSubject.SubjectId);

      // Delete all evaluations of this relation
      List<Task> deletions = [];
      foreach (var e in evaluations)
        deletions.Add(evalRepo.DeleteAsync(e.Id));
      await Task.WhenAll(deletions);

      await uow.CommitAsync().ConfigureAwait(false);
    } catch (DbUpdateException e) {
      throw new InvalidOperationException("Entity deletion failed.", e);
    }
  }

  public async Task<HasSubjectDetailModel> SaveAsync(HasSubjectDetailModel model, Guid userId) {
    HasSubjectEntity entity = hasSubjectModelMapper.MapToEntity(model, userId);

    await using IUnitOfWork uow = UnitOfWorkFactory.Create();
    IRepository<HasSubjectEntity> repository =
      uow.GetRepository<HasSubjectEntity, HasSubjectEntityMapper>();

    try {
      if (repository.Get()
            .SingleOrDefault(h => h.UserId == entity.UserId && h.SubjectId == entity.SubjectId)
          is not null)
        throw new InvalidOperationException("User is already in subject");

      var inserted = repository.Insert(entity);
      await uow.CommitAsync();
      return await GetAsync(inserted.Id) ??
             throw new InvalidOperationException("Cannot find inserted HasSubjectEntity");
    } catch (DbUpdateException ex) {
      throw new InvalidOperationException(ex.Message);
    }
  }

  public async Task<IEnumerable<HasSubjectListModel>> GetSubjectStudentsAsync(Guid subjectId) {
    await using IUnitOfWork uow = UnitOfWorkFactory.Create();

    IQueryable<HasSubjectEntity> query =
      uow.GetRepository<HasSubjectEntity, HasSubjectEntityMapper>().Get();

    query = query.Include(nameof(HasSubjectEntity.User));
    query = query.Include(nameof(HasSubjectEntity.Subject));

    IEnumerable<HasSubjectEntity> entities = await query.Where(s => s.SubjectId == subjectId)
      .ToListAsync().ConfigureAwait(false);

    return hasSubjectModelMapper
      .MapToListModel(entities)
      .Where(e => e.UserType == UserType.Student);
  }
}