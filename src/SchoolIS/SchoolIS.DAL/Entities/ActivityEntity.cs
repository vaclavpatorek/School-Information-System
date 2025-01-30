using System.ComponentModel.DataAnnotations.Schema;
using SchoolIS.Common.Enums;

namespace SchoolIS.DAL.Entities;

[Table("Activity")]
public record ActivityEntity : IEntity {
  public DateTime StartsFrom { get; set; }
  public DateTime EndsAt { get; set; }
  public ActivityType ActivityType { get; set; }

  public required SubjectEntity Subject { get; init; }
  public required Guid SubjectId { get; set; }
  public required UserEntity Creator { get; init; }
  public required Guid CreatorId { get; set; }
  public ICollection<EvaluationEntity> Evaluations { get; init; } = new List<EvaluationEntity>();
  public required RoomEntity Room { get; init; }
  public required Guid RoomId { get; set; }
  public Guid Id { get; set; }
}