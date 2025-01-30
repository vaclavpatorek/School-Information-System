using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolIS.DAL.Entities;

[Table("HasSubject")]
public record HasSubjectEntity : IEntity {
  public required UserEntity User { get; init; }
  public required SubjectEntity Subject { get; init; }
  public required Guid SubjectId { get; set; }
  public required Guid UserId { get; set; }
  public required Guid Id { get; set; }
}