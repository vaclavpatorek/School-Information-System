using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolIS.DAL.Entities;

[Table("Subject")]
public record SubjectEntity : IEntity {
  public required string Shortcut { get; set; }
  public required string Name { get; set; }
  public string? Info { get; set; }

  public ICollection<HasSubjectEntity> Users { get; init; } = new List<HasSubjectEntity>();
  public ICollection<ActivityEntity> Activities { get; init; } = new List<ActivityEntity>();
  public Guid Id { get; set; }
}