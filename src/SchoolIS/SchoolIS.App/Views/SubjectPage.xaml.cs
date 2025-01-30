using CommunityToolkit.Mvvm.Input;
using SchoolIS.App.ViewModels;
using SchoolIS.App.Views.Components;
using zoft.MauiExtensions.Controls;

namespace SchoolIS.App.Views;

public partial class SubjectPage : ContentPageBase {
  private string _selectedTab = "Activities";

  public string SelectedTab {
    get => _selectedTab;
    set {
      if (value != _selectedTab) {
        _selectedTab = value;
        OnPropertyChanged();
      }
    }
  }

  private bool _studentsSortAsc = true;
  private string _studentsSortBy = "login";

  public bool StudentsSortAsc {
    get => _studentsSortAsc;
    set {
      if (value != _studentsSortAsc) {
        _studentsSortAsc = value;
        OnPropertyChanged();
      }
    }
  }

  public string StudentsSortBy {
    get => _studentsSortBy;
    set {
      if (value == _studentsSortBy) return;

      _studentsSortBy = value;
      OnPropertyChanged();
      if (value != "login") OnPropertyChanged(nameof(StudentsSortLoginAsc));
      if (value != "name") OnPropertyChanged(nameof(StudentsSortNameAsc));
      if (value != "points") OnPropertyChanged(nameof(StudentsSortPointsAsc));
    }
  }

  public bool StudentsSortLoginAsc {
    get => StudentsSortBy != "login" || StudentsSortAsc;
    set {
      StudentsSortAsc = value;
      StudentsSortBy = "login";
    }
  }

  public bool StudentsSortNameAsc {
    get => StudentsSortBy != "name" || StudentsSortAsc;
    set {
      StudentsSortAsc = value;
      StudentsSortBy = "name";
    }
  }

  public bool StudentsSortPointsAsc {
    get => StudentsSortBy != "points" || StudentsSortAsc;
    set {
      StudentsSortAsc = value;
      StudentsSortBy = "points";
    }
  }

  public SubjectPage(SubjectViewModel subjectViewModel) : base(subjectViewModel) {
    InitializeComponent();
  }

  [RelayCommand]
  public static void SetEmpty(Textfield what) {
    what.Text = string.Empty;
  }

  [RelayCommand]
  public void SelectTab(string name) {
    if (SelectedTab != name)
      ((SubjectViewModel)ViewModel).ClearErrors();
    SelectedTab = name;
  }

  private string _addStudentLoginFilter = string.Empty;

  public string AddStudentLoginFilter {
    get => _addStudentLoginFilter;
    set {
      if (_addStudentLoginFilter != value) {
        _addStudentLoginFilter = value;
        OnPropertyChanged();
      }
    }
  }

  private void AutoCompleteEntry_TextChanged(object sender,
    AutoCompleteEntryTextChangedEventArgs e) {
    if (e.Reason == AutoCompleteEntryTextChangeReason.UserInput) {
      AddStudentLoginFilter = (sender as AutoCompleteEntry)!.Text;
    }
  }

  [RelayCommand]
  private async Task AddStudentButtonClickAsync(object param) {
    // Clear add student entry text.
    AddStudentEntry.Text = "";

    // Add student from ViewModel
    SubjectViewModel vm = (SubjectViewModel)ViewModel;
    await vm.AddStudentCommand.ExecuteAsync(param);
  }
}