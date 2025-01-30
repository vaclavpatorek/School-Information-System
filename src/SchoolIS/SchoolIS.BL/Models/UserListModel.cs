using SchoolIS.Common.Enums;

namespace SchoolIS.BL.Models;

public record UserListModel : ModelBase {
  public string FirstName { get; set; } = String.Empty;
  public string LastName { get; set; } = String.Empty;
  public string Login { get; set; } = String.Empty;
  public UserType Type { get; set; } = UserType.Teacher;
  public string FullName => $"{FirstName} {LastName}";
  public string FullNameLogin => $"{FullName} ({Login})";

  public static UserListModel Empty => new() {
    Id = Guid.NewGuid(),
    FirstName = String.Empty,
    LastName = String.Empty,
    Login = String.Empty,
    Type = UserType.Teacher
  };
}