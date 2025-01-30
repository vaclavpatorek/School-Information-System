namespace SchoolIS.BL.Models;

public record RoomListModel : ModelBase {
  public string Name { get; set; } = String.Empty;

  public static RoomListModel Empty => new() {
    Id = Guid.NewGuid(),
    Name = String.Empty,
  };
}