using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Seeds;

public static class ActivitySeeds {
  class RelativeDate(DayOfWeek day, int hours, int minutes) {
    private int Days { get; } = (int)day - 1;
    private int Hours { get; } = hours;
    private int Minutes { get; } = minutes;

    public DateTime AddToDateTime(DateTime dateTime) {
      return dateTime.AddDays(Days).AddHours(Hours).AddMinutes(Minutes);
    }
  }

  public static List<ActivityEntity> IpkActivities { get; private set; } = [];
  public static List<ActivityEntity> IdsActivities { get; private set; } = [];
  public static List<ActivityEntity> IfjActivities { get; private set; } = [];

  public static List<ActivityEntity> All =>
    new List<List<ActivityEntity>> { IpkActivities, IdsActivities, IfjActivities }
      .SelectMany(a => a).ToList();

  private static TimeSpan WEEK = TimeSpan.FromDays(7);
  private static DateTime middleDate = GetLastMonday();
  private static DateTime startDate = middleDate - WEEK * 6;
  private static DateTime endDate = middleDate + WEEK * 7;

  private static DateTime GetLastMonday() {
    var today = DateTime.Today;
    int daysUntilMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
    if (daysUntilMonday < 0)
      return today.AddDays(-6);
    return today.AddDays(-daysUntilMonday);
  }

  private static void SetupSubject(
    SubjectEntity subject, UserEntity teacher, RoomEntity lectureRoom,
    RelativeDate[] lectureSpan, RoomEntity exerciseRoom, RelativeDate[]? excerciesSpan,
    RoomEntity examRoom, RelativeDate[] examSpan, List<ActivityEntity> outAct
  ) {
    ActivityEntity subject_template = new() {
      StartsFrom = DateTime.Now,
      EndsAt = DateTime.Now,
      ActivityType = ActivityType.Lecture,
      Subject = subject,
      SubjectId = subject.Id,
      Creator = teacher,
      CreatorId = teacher.Id,
      Evaluations = [],
      Room = RoomSeeds.D105,
      RoomId = RoomSeeds.D105.Id,
      Id = Guid.NewGuid(),
    };

    // Generate lectures.
    for (DateTime d = startDate; d < endDate; d += WEEK) {
      outAct.Add(subject_template with {
        StartsFrom = lectureSpan[0].AddToDateTime(d),
        EndsAt = lectureSpan[1].AddToDateTime(d),
        ActivityType = ActivityType.Lecture,
        Room = lectureRoom,
        RoomId = lectureRoom.Id,
        Id = Guid.NewGuid(),
      });
    }

    // Generate exercises.
    for (DateTime d = startDate; d < endDate && excerciesSpan != null; d += WEEK * 2) {
      outAct.Add(subject_template with {
        StartsFrom = excerciesSpan[0].AddToDateTime(d),
        EndsAt = excerciesSpan[1].AddToDateTime(d),
        ActivityType = ActivityType.Exercise,
        Room = exerciseRoom,
        RoomId = exerciseRoom.Id,
        Id = Guid.NewGuid()
      });
    }

    // Generate exams
    foreach (var d in new List<DateTime> { middleDate, endDate + WEEK }) {
      outAct.Add(subject_template with {
        StartsFrom = examSpan[0].AddToDateTime(d),
        EndsAt = examSpan[1].AddToDateTime(d),
        ActivityType = ActivityType.Test,
        Room = examRoom,
        RoomId = examRoom.Id,
        Id = Guid.NewGuid(),
      });
    }
  }

  private static void SetupIpk() {
    SetupSubject(
      SubjectSeeds.Ipk,
      UserSeeds.Teachers[0],
      RoomSeeds.D105, [new(DayOfWeek.Tuesday, 10, 0), new(DayOfWeek.Tuesday, 11, 50)],
      RoomSeeds.O204, [new(DayOfWeek.Thursday, 7, 0), new(DayOfWeek.Thursday, 8, 50)],
      RoomSeeds.E112, [new(DayOfWeek.Wednesday, 17, 0), new(DayOfWeek.Wednesday, 19, 0)],
      IpkActivities);
  }

  private static void SetupIds() {
    SetupSubject(
      SubjectSeeds.Ids,
      UserSeeds.Teachers[1],
      RoomSeeds.D105, [new(DayOfWeek.Monday, 8, 0), new(DayOfWeek.Monday, 9, 50)],
      RoomSeeds.O204, [new(DayOfWeek.Tuesday, 7, 0), new(DayOfWeek.Tuesday, 9, 50)],
      RoomSeeds.E112, [new(DayOfWeek.Friday, 12, 0), new(DayOfWeek.Friday, 14, 0)],
      IdsActivities);
  }

  private static void SetupIfj() {
    SetupSubject(
      SubjectSeeds.Ifj,
      UserSeeds.Teachers[2],
      RoomSeeds.D105, [new(DayOfWeek.Thursday, 11, 0), new(DayOfWeek.Thursday, 13, 50)],
      RoomSeeds.O204, null,
      RoomSeeds.E112, [new(DayOfWeek.Monday, 15, 0), new(DayOfWeek.Monday, 16, 0)],
      IfjActivities);
  }

  private static void SetupActivities() {
    SetupIpk();
    SetupIds();
    SetupIfj();
  }

  static ActivitySeeds() {
    SetupActivities();
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    List<List<ActivityEntity>> activities = [IpkActivities, IdsActivities, IfjActivities];

    modelBuilder.Entity<ActivityEntity>()
      .HasData(
        activities.SelectMany(l => l).Select(a => a with {
          Subject = null!,
          Creator = null!,
          Room = null!,
          Evaluations = Array.Empty<EvaluationEntity>()
        })
      );
  }
}