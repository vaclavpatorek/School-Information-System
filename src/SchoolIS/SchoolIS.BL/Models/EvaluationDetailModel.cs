using SchoolIS.Common.Enums;

namespace SchoolIS.BL.Models;

public record EvaluationDetailModel : ModelBase {
  public Guid ActivityId { get; set; }
  public Guid StudentId { get; set; }
  public string StudentLogin = String.Empty;
  public string StudentFirstName = String.Empty;
  public string StudentLastName = String.Empty;
  public string? Note { get; set; }
  public uint Points { get; set; } = 0;
  public bool Attended { get; set; }

  public static EvaluationDetailModel Empty => new() {
    Id = Guid.NewGuid(),
    ActivityId = Guid.Empty,
    StudentId = Guid.Empty,
    StudentLogin = String.Empty,
    StudentFirstName = String.Empty,
    StudentLastName = String.Empty,
    Note = null,
    Points = 0,
    Attended = false
  };
}