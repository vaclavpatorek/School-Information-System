using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;

namespace SchoolIS.Common.Tests.Seeds;

public static class UserSeeds {
  public static UserEntity User = new() {
    Id = Guid.Parse("03b3902d-7d4f-4213-9cf0-112348f56238"),
    FirstName = "FIRST",
    LastName = "LAST",
    PhotoUrl = "TEST_URL.jpg",
    Login = "xTest00"
  };

  public static UserEntity EmptyUser = new() {
    Login = default!,
    FirstName = default!,
    LastName = default!,
    PhotoUrl = default!,
    Email = default!,
    UserType = default!,
    PhoneNumber = default!,
    Subjects = default!,
    Activities = default!,
    Evaluations = default!,
    Id = default
  };

  public static UserEntity User2 = User with {
    Id = Guid.Parse("791878B2-3355-4683-BD48-7D950EEA54B8"),
    Login = "xuser2",
    UserType = UserType.Student,
    Subjects = new List<HasSubjectEntity>(),
    Evaluations = null!,
    Activities = null!,
  };

  public static UserEntity UserDelete = User with {
    Id = Guid.Parse("819B659B-91CF-402C-95E6-EC482A193C6D"),
    Login = "xdelete",
    Subjects = null!,
    Evaluations = null!,
    Activities = null!,
  };

  public static UserEntity UserNoRefs = User with {
    Id = Guid.Parse("04b3902d-7d4f-4213-9cf0-112348f56238"),
    Login = "xTest01",
    Activities = Array.Empty<ActivityEntity>(),
    Evaluations = Array.Empty<EvaluationEntity>(),
    Subjects = Array.Empty<HasSubjectEntity>(),
  };

  static UserSeeds() {
    User.Activities.Add(ActivitySeeds.Activity);
    User.Activities.Add(ActivitySeeds.Activity2);

    User.Evaluations.Add(EvaluationSeeds.Evaluation);
    User.Evaluations.Add(EvaluationSeeds.Evaluation2);

    User.Subjects.Add(HasSubjectSeeds.HasSubject);
    User.Subjects.Add(HasSubjectSeeds.HasSubjectDelete);
    User2.Subjects.Add(HasSubjectSeeds.HasSubject2);
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<UserEntity>()
      .HasData(
        User with {
          Activities = Array.Empty<ActivityEntity>(),
          Evaluations = Array.Empty<EvaluationEntity>(),
          Subjects = Array.Empty<HasSubjectEntity>(),
        },
        User2 with {
          Subjects = Array.Empty<HasSubjectEntity>(),
        },
        UserNoRefs, UserDelete);
  }
}