using System.Collections.ObjectModel;
using SchoolIS.Common.Enums;

namespace SchoolIS.BL.Models;

public record ActivityDetailModel : ModelBase {
  public Guid RoomId { get; set; }
  public Guid CreatorId { get; set; }
  public Guid SubjectId { get; set; }
  public DateTime StartsFrom { get; set; }
  public DateTime EndsAt { get; set; }
  public ActivityType ActivityType { get; set; }
  public string CreatorFirstName { get; set; } = String.Empty;
  public string CreatorLastName { get; set; } = String.Empty;
  public string RoomName { get; set; } = String.Empty;
  public string SubjectName { get; set; } = String.Empty;
  public string SubjectShortcut { get; set; } = String.Empty;

  public ObservableCollection<EvaluationListModel> Evaluations { get; set; } = [];

  public static ActivityDetailModel Empty => new() {
    Id = Guid.NewGuid(),
    RoomId = Guid.Empty,
    CreatorId = Guid.Empty,
    SubjectId = Guid.Empty,
    StartsFrom = DateTime.Now,
    EndsAt = DateTime.Now,
    ActivityType = ActivityType.Exercise,
    CreatorFirstName = String.Empty,
    CreatorLastName = String.Empty,
    RoomName = String.Empty,
    Evaluations = [],
  };
}