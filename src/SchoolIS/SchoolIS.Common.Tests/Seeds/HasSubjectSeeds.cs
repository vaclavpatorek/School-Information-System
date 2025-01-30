using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;

namespace SchoolIS.Common.Tests.Seeds;

public static class HasSubjectSeeds {
  public static HasSubjectEntity HasSubject = new() {
    Id = Guid.Parse("6663902d-7d4f-4213-9cf0-112348f56238"),
    UserId = UserSeeds.User.Id,
    User = UserSeeds.User,
    SubjectId = SubjectSeeds.Subject.Id,
    Subject = SubjectSeeds.Subject
  };

  public static HasSubjectEntity HasSubject2 = new() {
    Id = Guid.Parse("4180B1C3-2F83-4EA0-B4F8-DE6A9D2E159C"),
    SubjectId = SubjectSeeds.Subject.Id,
    Subject = SubjectSeeds.Subject,
    UserId = UserSeeds.User2.Id,
    User = UserSeeds.User2,
  };

  public static readonly HasSubjectEntity HasSubjectDelete = new() {
    Id = Guid.Parse("7BF21D0B-466A-457A-BE5D-86BCD8DAFDC4"),
    SubjectId = SubjectSeeds.Subject2.Id,
    Subject = SubjectSeeds.Subject2,
    UserId = UserSeeds.User.Id,
    User = UserSeeds.User,
  };

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<HasSubjectEntity>().HasData(
      HasSubject with {
        User = null!,
        Subject = null!
      },
      HasSubject2 with {
        User = null!,
        Subject = null!
      },
      HasSubjectDelete with {
        User = null!,
        Subject = null!
      }
    );
  }
}