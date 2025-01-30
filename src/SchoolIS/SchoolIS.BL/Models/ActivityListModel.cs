using SchoolIS.Common.Enums;

namespace SchoolIS.BL.Models;

public record ActivityListModel : ModelBase {
  public Guid RoomId { get; set; }
  public Guid CreatorId { get; set; }
  public Guid SubjectId { get; set; }
  public DateTime StartsFrom { get; set; }
  public DateTime EndsAt { get; set; }
  public ActivityType ActivityType { get; set; }
  public string CreatorFirstName { get; set; } = string.Empty;
  public string CreatorLastName { get; set; } = string.Empty;
  public string RoomName { get; set; } = string.Empty;

  public static ActivityListModel Empty = new() {
    Id = Guid.NewGuid(),
    RoomId = Guid.Empty,
    CreatorId = Guid.Empty,
    SubjectId = Guid.Empty,
    StartsFrom = DateTime.Now,
    EndsAt = DateTime.Now,
    ActivityType = ActivityType.Exercise,
    CreatorFirstName = string.Empty,
    CreatorLastName = string.Empty,
    RoomName = string.Empty
  };
}