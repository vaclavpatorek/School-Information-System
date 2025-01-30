using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Seeds;

public static class EvaluationSeeds {
  private static readonly string[] _notes = [
    "Fuj! Další rok to snad vyjde ;)",
    "No moc tam toho dobře není.",
    "Hmm..",
    "Malé chybičky.",
    "Výborné!"
  ];

  public static List<EvaluationEntity> IpkEvaluations { get; } = [];
  public static List<EvaluationEntity> IdsEvaluations { get; } = [];
  public static List<EvaluationEntity> IfjEvaluations { get; } = [];

  public static List<EvaluationEntity> AllEvaluations =>
    new List<List<EvaluationEntity>> { IpkEvaluations, IdsEvaluations, IfjEvaluations }
      .SelectMany(x => x).ToList();

  private static void SetupSubject(SubjectEntity subject, IEnumerable<ActivityEntity> activities,
    List<EvaluationEntity> outEvals) {
    var users = subject.Users.Select(e => e.User).Where(u => u.UserType == UserType.Student);
    Random random = new(42);

    var evals =
      from user in users.Select((s, i) => new { s, i })
      from activity in activities.Select((a, i) => new { a, i })
      select new {
        Student = user.s,
        Activity = activity.a,
        Index = (activity.i * users.Count()) + user.i
      };

    foreach (var e in evals) {
      var i = (uint)(e.Index % _notes.Length);
      outEvals.Add(new() {
        Activity = e.Activity,
        ActivityId = e.Activity.Id,
        Student = e.Student,
        StudentId = e.Student.Id,
        Points = (uint)(i * random.NextDouble() * 5),
        Note = _notes[i],
        Id = Guid.NewGuid()
      });
    }
  }

  private static bool IsExercise(ActivityEntity a) => a.ActivityType == ActivityType.Exercise;
  private static bool IsTest(ActivityEntity a) => a.ActivityType == ActivityType.Test;

  private static void SetupEvaluations() {
    SetupSubject(SubjectSeeds.Ipk, ActivitySeeds.IpkActivities.Where(IsExercise), IpkEvaluations);
    SetupSubject(SubjectSeeds.Ids, ActivitySeeds.IdsActivities.Where(IsExercise), IdsEvaluations);
    SetupSubject(SubjectSeeds.Ifj, ActivitySeeds.IfjActivities.Where(IsExercise), IfjEvaluations);

    SetupSubject(SubjectSeeds.Ipk, ActivitySeeds.IpkActivities.Where(IsTest), IpkEvaluations);
    SetupSubject(SubjectSeeds.Ids, ActivitySeeds.IdsActivities.Where(IsTest), IdsEvaluations);
    SetupSubject(SubjectSeeds.Ifj, ActivitySeeds.IfjActivities.Where(IsTest), IfjEvaluations);
  }

  static EvaluationSeeds() {
    SetupEvaluations();
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<EvaluationEntity>()
      .HasData(
        AllEvaluations.Select(e => e with {
          Activity = null!,
          Student = null!,
        })
      );
  }
}