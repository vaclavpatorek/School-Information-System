using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;

namespace SchoolIS.Common.Tests.Seeds;

public static class SubjectSeeds {
  public static SubjectEntity Subject = new() {
    Id = Guid.Parse("77b3902d-7d4f-4213-9cf0-112348f56238"),
    Name = "TEST SUBJECT",
    Shortcut = "TS1",
    Info = null
  };

  public static SubjectEntity EmptySubject = new() {
    Shortcut = default!,
    Name = default!,
    Info = default!,
    Users = default!,
    Activities = default!,
    Id = default
  };

  public static SubjectEntity SubjectDelete = Subject with {
    Id = Guid.Parse("C4219FFD-759D-458F-94DB-F268EC1D73FD"),
    Name = "TO DELETE",
    Activities = Array.Empty<ActivityEntity>(),
    Users = Array.Empty<HasSubjectEntity>(),
    Shortcut = "TS2",
  };

  public static SubjectEntity Subject2 = Subject with {
    Id = Guid.Parse("FB2045F2-367A-4151-B243-AB3B7AE4E40C"),
    Activities = new List<ActivityEntity>(),
    Users = new List<HasSubjectEntity>(),
    Shortcut = "TS3",
  };

  public static SubjectEntity SubjectNoRefs = Subject with {
    Id = Guid.Parse("CFBFB7C4-600B-45F7-8CB2-26492F1B0347"),
    Activities = Array.Empty<ActivityEntity>(),
    Users = Array.Empty<HasSubjectEntity>(),
    Shortcut = "TS4",
  };

  static SubjectSeeds() {
    Subject.Activities.Add(ActivitySeeds.Activity);
    Subject.Activities.Add(ActivitySeeds.Activity2);
    Subject2.Activities.Add(ActivitySeeds.Activity3);

    Subject.Users.Add(HasSubjectSeeds.HasSubject);
    Subject.Users.Add(HasSubjectSeeds.HasSubject2);

    Subject2.Users.Add(HasSubjectSeeds.HasSubjectDelete);
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<SubjectEntity>()
      .HasData(
        Subject with {
          Activities = Array.Empty<ActivityEntity>(),
          Users = Array.Empty<HasSubjectEntity>(),
        },
        Subject2 with {
          Activities = Array.Empty<ActivityEntity>(),
          Users = Array.Empty<HasSubjectEntity>()
        },
        SubjectNoRefs,
        SubjectDelete
      );
  }
}