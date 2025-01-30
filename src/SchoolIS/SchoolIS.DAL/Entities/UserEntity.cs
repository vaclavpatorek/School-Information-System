using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;

namespace SchoolIS.DAL.Entities;

[Table("User")]
[Index(nameof(Login), IsUnique = true)]
public record UserEntity : IEntity {
  public required string Login { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public string? Password { get; set; }
  public string? PhotoUrl { get; set; }
  public string? Email { get; set; }
  public UserType UserType { get; set; }
  public string? PhoneNumber { get; set; }


  public ICollection<HasSubjectEntity> Subjects { get; init; } = new List<HasSubjectEntity>();
  public ICollection<ActivityEntity> Activities { get; init; } = new List<ActivityEntity>();
  public ICollection<EvaluationEntity> Evaluations { get; init; } = new List<EvaluationEntity>();

  [Key] public Guid Id { get; set; }
}