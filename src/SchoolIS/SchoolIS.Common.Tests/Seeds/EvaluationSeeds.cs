using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;

namespace SchoolIS.Common.Tests.Seeds;

public static class EvaluationSeeds {
  public static EvaluationEntity Evaluation = new() {
    Id = Guid.Parse("7773902d-7d4f-4213-9cf0-112348f56238"),
    Note = "TEST NOTE",
    Points = 8,
    StudentId = UserSeeds.User.Id,
    Student = UserSeeds.User,
    ActivityId = ActivitySeeds.Activity.Id,
    Activity = ActivitySeeds.Activity
  };

  public static EvaluationEntity Evaluation2 = new() {
    Id = Guid.Parse("D70C9FCB-44FE-4EF4-B0E4-D83EDF5A7178"),
    Note = "TEST NOTE",
    Points = 8,
    StudentId = UserSeeds.User.Id,
    Student = UserSeeds.User,
    ActivityId = ActivitySeeds.Activity3.Id,
    Activity = ActivitySeeds.Activity3
  };

  public static readonly EvaluationEntity EvaluationUpdate = Evaluation with {
    Id = Guid.Parse("95C4C591-36B7-4AD2-AAB0-0EBFA8FEF6B5"), 
    StudentId = UserSeeds.UserDelete.Id,
    ActivityId = ActivitySeeds.ActivityDelete.Id,
    Student = null!, 
    Activity = null!
  };

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<EvaluationEntity>().HasData(
      Evaluation with { Student = null!, Activity = null! },
      Evaluation2 with { Student = null!, Activity = null! },
      EvaluationUpdate
    );
  }
}