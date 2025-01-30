using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;

namespace SchoolIS.Common.Tests.Seeds;

public static class RoomSeeds {
  public static RoomEntity Room = new() {
    Id = Guid.Parse("0003902d-7d4f-4213-9cf0-112348f56238"),
    Name = "TEST ROOM"
  };

  public static RoomEntity RoomDelete = new() {
    Id = Guid.Parse("30BBB7BA-3A66-4C71-846E-0258A97B21D9"),
    Name = "ROOM TO DELETION"
  };

  public static RoomEntity EmptyRoom = new() {
    Id = default,
    Name = default!
  };


  static RoomSeeds() {
    Room.Activities.Add(ActivitySeeds.Activity);
    Room.Activities.Add(ActivitySeeds.Activity2);
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<RoomEntity>()
      .HasData(Room with { Activities = Array.Empty<ActivityEntity>() }, RoomDelete);
  }
}