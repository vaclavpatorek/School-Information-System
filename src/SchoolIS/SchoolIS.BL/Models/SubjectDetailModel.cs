using System.Collections.ObjectModel;

namespace SchoolIS.BL.Models;

public record SubjectDetailModel : ModelBase {
  public string Shortcut { get; set; } = String.Empty;
  public string Name { get; set; } = String.Empty;
  public string? Info { get; set; } = String.Empty;

  public ObservableCollection<HasSubjectListModel> Students { get; set; } = [];
  public ObservableCollection<HasSubjectListModel> Teachers { get; set; } = [];

  public static SubjectDetailModel Empty => new() {
    Id = Guid.NewGuid(),
    Shortcut = String.Empty,
    Name = String.Empty,
    Info = String.Empty,
    Students = [],
    Teachers = [],
  };
}