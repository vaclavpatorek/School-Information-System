using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Seeds;

namespace SchoolIS.DAL;

// ReSharper disable once InconsistentNaming
public class SchoolISDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
  : DbContext(contextOptions) {
  public DbSet<ActivityEntity> ActivityEntities => Set<ActivityEntity>();
  public DbSet<HasSubjectEntity> HasSubjectEntities => Set<HasSubjectEntity>();
  public DbSet<EvaluationEntity> EvaluationEntities => Set<EvaluationEntity>();
  public DbSet<RoomEntity> RoomEntities => Set<RoomEntity>();
  public DbSet<SubjectEntity> SubjectEntities => Set<SubjectEntity>();
  public DbSet<UserEntity> UserEntities => Set<UserEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<UserEntity>()
      .HasMany(i => i.Evaluations)
      .WithOne(i => i.Student)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<UserEntity>()
      .HasMany(i => i.Subjects)
      .WithOne(i => i.User)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<UserEntity>()
      .HasMany(i => i.Activities)
      .WithOne(i => i.Creator)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<SubjectEntity>()
      .HasMany(i => i.Users)
      .WithOne(i => i.Subject)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<SubjectEntity>()
      .HasMany(i => i.Activities)
      .WithOne(i => i.Subject)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ActivityEntity>()
      .HasMany(i => i.Evaluations)
      .WithOne(i => i.Activity)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<RoomEntity>()
      .HasMany(i => i.Activities)
      .WithOne(i => i.Room)
      .OnDelete(DeleteBehavior.Restrict);

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