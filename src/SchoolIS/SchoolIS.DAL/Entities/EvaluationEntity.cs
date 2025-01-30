using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolIS.DAL.Entities;

[Table("Evaluation")]
public record EvaluationEntity : IEntity {
  public uint? Points { get; set; }
  public string? Note { get; set; }

  public required UserEntity Student { get; init; }
  public required Guid StudentId { get; set; }
  public required ActivityEntity Activity { get; init; }
  public required Guid ActivityId { get; set; }
  public Guid Id { get; set; }
}