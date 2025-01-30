using SchoolIS.App.ViewModels;

namespace SchoolIS.App.Views;

public partial class TimetablePage: ContentPageBase {
  public TimetablePage(TimetableViewModel timetableViewModel) : base(timetableViewModel) {
    InitializeComponent();
  }
}