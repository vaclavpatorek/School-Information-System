using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Tests.Seeds;
using SchoolIS.DAL;

namespace SchoolIS.Common.Tests;

public class SchoolISTestingDbContext(
  DbContextOptions contextOptions,
  bool seedTestingData = false) : SchoolISDbContext(contextOptions, false) {
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    if (seedTestingData) {
      UserSeeds.Seed(modelBuilder);
      SubjectSeeds.Seed(modelBuilder);
      RoomSeeds.Seed(modelBuilder);
      ActivitySeeds.Seed(modelBuilder);
      EvaluationSeeds.Seed(modelBuilder);
      HasSubjectSeeds.Seed(modelBuilder);
    }
  }
}