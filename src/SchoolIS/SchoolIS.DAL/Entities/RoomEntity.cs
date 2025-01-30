using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolIS.DAL.Entities;

[Table("Room")]
public record RoomEntity : IEntity {
  public required string Name { get; set; }

  public ICollection<ActivityEntity> Activities { get; init; } = new List<ActivityEntity>();
  public Guid Id { get; set; }
}