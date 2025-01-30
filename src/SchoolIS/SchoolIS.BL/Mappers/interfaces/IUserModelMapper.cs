using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Mappers.Interfaces;

public interface IUserModelMapper : IModelMapper<UserEntity, UserListModel, UserDetailModel> { }