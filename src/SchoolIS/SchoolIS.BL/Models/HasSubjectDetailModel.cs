using SchoolIS.Common.Enums;

namespace SchoolIS.BL.Models;

public record HasSubjectDetailModel : ModelBase {
  public required Guid SubjectId { get; set; }
  public required Guid UserId { get; set; }
  public required string SubjectName { get; set; }
  public required string SubjectShortcut { get; set; }
  public required string UserLogin { get; set; }
  public required string UserFirstName { get; set; }
  public required string UserLastName { get; set; }
  public required UserType UserType { get; set; }
  public int TotalPoints { get; set; } = 0;

  public static HasSubjectDetailModel Empty = new() {
    Id = Guid.NewGuid(),
    SubjectId = Guid.Empty,
    UserId = Guid.Empty,
    SubjectName = String.Empty,
    SubjectShortcut = String.Empty,
    UserLogin = String.Empty,
    UserFirstName = String.Empty,
    UserLastName = String.Empty,
    UserType = UserType.Student,
    TotalPoints = 0
  };
}