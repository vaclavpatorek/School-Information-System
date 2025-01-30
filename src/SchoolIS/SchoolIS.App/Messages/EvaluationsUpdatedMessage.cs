namespace SchoolIS.App.Messages;

public record EvaluationsUpdatedMessage {
  public Guid SubjectId { get; set; } = Guid.Empty;
  public Guid ActivityId { get; set; } = Guid.Empty;
}