using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.UnitOfWork;

namespace SchoolIS.BL.Facades;

public class UserFacade(
  IUnitOfWorkFactory unitOfWorkFactory,
  IUserModelMapper userModelMapper) :
  FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>(
    unitOfWorkFactory,
    userModelMapper), IUserFacade {
  protected override ICollection<string> IncludesNavigationPathDetail =>
    new[] {
      $"{nameof(UserEntity.Subjects)}.{nameof(HasSubjectEntity.Subject)}",
      $"{nameof(UserEntity.Activities)}",
      $"{nameof(UserEntity.Evaluations)}"
    };

  public async Task<UserDetailModel?> GetUserByLogin(string login) {
    await using IUnitOfWork uow = UnitOfWorkFactory.Create();

    IQueryable<UserEntity> query = uow.GetRepository<UserEntity, UserEntityMapper>().Get();

    foreach (string includePath in IncludesNavigationPathDetail) {
      query = query.Include(includePath);
    }

    UserEntity? entity = await query.SingleOrDefaultAsync(user => user.Login == login).ConfigureAwait(false);

    return entity is null
      ? null
      : userModelMapper.MapToDetailModel(entity);
  }
}