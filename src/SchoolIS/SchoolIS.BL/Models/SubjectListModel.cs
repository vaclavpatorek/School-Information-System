namespace SchoolIS.BL.Models;

public record SubjectListModel : ModelBase {
  public string Shortcut { get; set; } = String.Empty;
  public string Name { get; set; } = String.Empty;
  public uint StudentCount { get; set; } = 0;

  public static SubjectListModel Empty => new() {
    Id = Guid.NewGuid(),
    Shortcut = String.Empty,
    Name = String.Empty,
    StudentCount = 0
  };
}