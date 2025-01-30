using System.Security.Authentication;
using SchoolIS.BL.Models;

namespace SchoolIS.App.Services.Interfaces;

public interface IAuthenticationService {
  ///<summary>Throws <see cref="AuthenticationException"/> when invalid login or password provided.</summary>
  /// <exception cref="AuthenticationException"></exception>
  void LoginUser(UserDetailModel? user, string password);

  void LogoutUser();

  void LoginWithoutPasswordValidation(UserDetailModel user);
  
  bool IsAdmin(string login, string password);

  ///<summary>Retrieve currently authenticated user. If no user is authenticated, <see cref="AuthenticationException"/> is thrown.</summary>
  /// <exception cref="AuthenticationException"></exception>
  UserDetailModel GetLoggedInUser();
}