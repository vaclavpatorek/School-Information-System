using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Seeds;
public static class RoomSeeds {
  public static RoomEntity D105 = new() {
    Id = Guid.Parse("5b770b8d-a002-411d-9238-24f7f13093b4"),
    Name = "D105",
    Activities = null!,
  };

  public static RoomEntity O204 = new() {
    Id = Guid.Parse("1122d756-87ec-4591-96e8-99fa01625765"),
    Name = "O204",
    Activities = null!,
  };

  public static RoomEntity E112 = new() {
    Id = Guid.Parse("a6de8ff8-8111-4506-8138-36bdb6e04658"),
    Name = "E112",
    Activities = null!,
  };

  static RoomSeeds() {

  }
  public static void Seed(this ModelBuilder modelBuilder) {
    RoomEntity[] rooms = [D105, O204, E112];

    modelBuilder.Entity<RoomEntity>()
      .HasData(
        rooms.Select(r => r with { 
          Activities = Array.Empty<ActivityEntity>()
        })
      );
  }
}
