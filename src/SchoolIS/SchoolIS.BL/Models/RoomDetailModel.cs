namespace SchoolIS.BL.Models;

public record RoomDetailModel : ModelBase {
  public string Name { get; set; } = String.Empty;

  public static RoomDetailModel Empty => new() {
    Id = Guid.NewGuid(),
    Name = String.Empty,
  };
}