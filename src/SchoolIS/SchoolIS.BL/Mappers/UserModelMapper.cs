using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers;

public class UserModelMapper(IHasSubjectModelMapper hasSubjectModelMapper) :
  ModelMapperBase<UserEntity, UserListModel, UserDetailModel>, IUserModelMapper {
  public override UserListModel MapToListModel(UserEntity? entity) =>
    entity is null
      ? UserListModel.Empty
      : new UserListModel {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Type = entity.UserType,
        Login = entity.Login,
      };

  public override UserDetailModel MapToDetailModel(UserEntity? entity) =>
    entity is null
      ? UserDetailModel.Empty
      : new UserDetailModel {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Type = entity.UserType,
        Login = entity.Login,
        PhoneNumber = entity.PhoneNumber,
        PhotoUrl = entity.PhotoUrl,
        Email = entity.Email,
        Password = entity.Password,
        Subjects = hasSubjectModelMapper.MapToListModel(entity.Subjects).ToObservableCollection()
      };

  public override UserEntity MapToEntity(UserDetailModel model) =>
    new() {
      Id = model.Id,
      FirstName = model.FirstName,
      LastName = model.LastName,
      Login = model.Login,
      UserType = model.Type,
      PhoneNumber = model.PhoneNumber,
      PhotoUrl = model.PhotoUrl,
      Email = model.Email,
      Password = model.Password,
    };
}