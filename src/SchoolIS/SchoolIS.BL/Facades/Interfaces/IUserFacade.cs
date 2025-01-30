using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;

namespace SchoolIS.BL.Facades.Interfaces;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel> {
  Task<UserDetailModel?> GetUserByLogin(string login);
}