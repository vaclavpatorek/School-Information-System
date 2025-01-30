using CommunityToolkit.Mvvm.Input;
using SchoolIS.App.ViewModels;

namespace SchoolIS.App.Views;

public partial class EvaluationPage : ContentPageBase {
  public EvaluationPage(EvaluationViewModel evaluationViewModel) : base(evaluationViewModel) {
    InitializeComponent();
  }

  [RelayCommand]
  private void GlobalNoteButtonClicked(string note) {
    GlobalNoteTextfield.Text = string.Empty;
    ((EvaluationViewModel)ViewModel).AddGlobalNoteCommand.Execute(note);
  }
}