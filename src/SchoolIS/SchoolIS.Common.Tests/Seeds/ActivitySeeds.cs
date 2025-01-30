using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;

namespace SchoolIS.Common.Tests.Seeds;

public static class ActivitySeeds {

  public static ActivityEntity EmptyActivity = new() {
    StartsFrom = default,
    EndsAt = default,
    ActivityType = default,
    Subject = null!,
    SubjectId = default,
    Creator = null!,
    CreatorId = default,
    Evaluations = null!,
    Room = null!,
    RoomId = default,
    Id = default,
  };

  public static ActivityEntity Activity = new() {
    Id = Guid.Parse("88b3902d-7d4f-4213-9cf0-112348f56238"),
    StartsFrom = DateTime.Parse("May 7, 2024 13:00:00"),
    EndsAt = DateTime.Parse("May 7, 2024 14:50:00"),
    CreatorId = UserSeeds.User.Id,
    Creator = UserSeeds.User,
    RoomId = RoomSeeds.Room.Id,
    Room = RoomSeeds.Room,
    SubjectId = SubjectSeeds.Subject.Id,
    Subject = SubjectSeeds.Subject
  };

  public static ActivityEntity Activity2 = Activity with {
    Id = Guid.Parse("09384056-169E-4E66-A9B5-F860E2279BF7"),
    Evaluations = Array.Empty<EvaluationEntity>(),
    ActivityType = ActivityType.Meeting,
    StartsFrom = DateTime.Parse("May 9, 2024 11:00:00"),
    EndsAt = DateTime.Parse("May 9, 2024 12:50:00"),
  };

  public static ActivityEntity Activity3 = Activity with {
    Id = Guid.Parse("7F47AD8C-B8FE-440B-8C3B-632D76543E7C"),
    Evaluations = new List<EvaluationEntity>(),
    ActivityType = ActivityType.Test,
    StartsFrom = DateTime.Parse("May 20, 2024 11:00:00"),
    EndsAt = DateTime.Parse("May 20, 2024 12:50:00"),
    SubjectId = SubjectSeeds.Subject2.Id,
    Subject = SubjectSeeds.Subject
  };

  public static ActivityEntity ActivityDelete = Activity with {
    Id = Guid.Parse("31C201BD-9BA1-46EB-9BF2-C50463632932"),
    SubjectId = SubjectSeeds.SubjectDelete.Id,
    CreatorId = UserSeeds.UserDelete.Id,
    Evaluations = Array.Empty<EvaluationEntity>(),
    Creator = null!, Subject = null!, Room = null!
  };


  static ActivitySeeds() {
    Activity.Evaluations.Add(EvaluationSeeds.Evaluation);
    Activity3.Evaluations.Add(EvaluationSeeds.Evaluation2);
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<ActivityEntity>()
      .HasData(
        Activity with {
          Creator = null!, Subject = null!, Room = null!,
          Evaluations = Array.Empty<EvaluationEntity>()
        },
        Activity2 with {
          Creator = null!, Subject = null!, Room = null!,
          Evaluations = Array.Empty<EvaluationEntity>()
        },
        Activity3 with {
          Creator = null!, Subject = null!, Room = null!,
          Evaluations = Array.Empty<EvaluationEntity>()
        },
        ActivityDelete
      );
  }
}