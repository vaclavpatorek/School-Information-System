using System.Security.Authentication;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL;
using SchoolIS.BL.Models;
using SchoolIS.Common.Enums;

namespace SchoolIS.App.Services;

public class AuthenticationService : IAuthenticationService {
  private const string StudentLoginFailed = "System is not accessible to students.";

  public const string InvalidCredentials =
    "AuthenticatedUser name or password doesn't match. Check your credentials.";

  private const string AdminHashedPassword = "$2a$11$UOOQwQUX6qCZjkjWVboL/OLULsK8sg0FZLm8t8iSR8ndeTcswQ4y2";

  private UserDetailModel? AuthenticatedUser { get; set; }

  public void LoginUser(UserDetailModel? user, string password) {
    // Invalid login
    if (user == null) throw new AuthenticationException(InvalidCredentials);

    // Do not allow student to login
    if (user.Type == UserType.Student) throw new AuthenticationException(StudentLoginFailed);

    // Password validation
    if (!VerifyPassword(password, user.Password ?? "pass".Hash()))
      throw new AuthenticationException(InvalidCredentials);

    AuthenticatedUser = user;
  }

  public void LogoutUser() {
    AuthenticatedUser = null;
  }

  public void LoginWithoutPasswordValidation(UserDetailModel user) {
    AuthenticatedUser = user;
  }

  public bool IsAdmin(string login, string password) {
    return login == "admin" && VerifyPassword(password, AdminHashedPassword);
  }

  public UserDetailModel GetLoggedInUser() {
    if (AuthenticatedUser == null)
      throw new AuthenticationException("User is not logged in");
    return AuthenticatedUser;
  }

  private bool VerifyPassword(string password, string hashedPassword) {
    return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
  }
}