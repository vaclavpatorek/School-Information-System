namespace SchoolIS.BL.Models;

public record EvaluationListModel : ModelBase {
  public Guid ActivityId { get; set; }
  public Guid StudentId { get; set; }
  public string StudentLogin { get; set; } = String.Empty;
  public string StudentFirstName { get; set; } = String.Empty;
  public string StudentLastName { get; set; } = String.Empty;
  public string? Note { get; set; }
  public uint Points { get; set; } = 0;
  public bool Attended { get; set; }

  public static EvaluationListModel Empty => new() {
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