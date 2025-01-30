using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SchoolIS.App.Messages;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.BL;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;

namespace SchoolIS.App.ViewModels;

public partial class EvaluationViewModel(
  IMessengerService messengerService,
  INavigationService navigation,
  IAuthenticationService authenticationService,
  IActivityFacade activityFacade,
  IEvaluationFacade evaluationFacade,
  IHasSubjectFacade hasSubjectFacade)
  : ViewModelBase(messengerService, navigation, authenticationService), IQueryAttributable {
  public Guid ActivityId { get; set; } = Guid.Empty;

  [ObservableProperty] private ActivityDetailModel _activity = null!;

  // Message that notifies the user about result of some action (error or success).
  [ObservableProperty] private string _message = string.Empty;

  // Marks if the Message is error or not.
  [ObservableProperty] private bool _isError;

  [RelayCommand]
  private void AddGlobalNote(string note) {
    foreach (var eval in Activity.Evaluations) {
      eval.Note = note;
    }
  }

  [RelayCommand]
  private async Task SaveEvaluationsAsync() {
    SetMessage("Saving...");

    try {
      // FIXME: It would be best, if we could create a transaction here and rollback if needed.
      // Save each evaluation in the list of evaluations.
      List<Task> saveTasks = [];
      foreach (var e in Activity.Evaluations) {
        var newEvaluation = EvaluationDetailModel.Empty with {
          ActivityId = Activity.Id,
          Points = e.Points,
          Note = e.Note,
          StudentId = e.StudentId,
          Attended = true,
          Id = e.Id,
        };
        saveTasks.Add(evaluationFacade.SaveAsync(newEvaluation));
      }

      await Task.WhenAll(saveTasks);

      MessengerService.Send(new EvaluationsUpdatedMessage {
        ActivityId = Activity.Id,
        SubjectId = Activity.SubjectId,
      });
      SetMessage("Evaluations saved.");
    } catch (InvalidOperationException e) {
      Trace.WriteLine($"Evaluation save failed. Exception: {e}");
      SetMessage("Saving failed.", isError: true);
    }

    Activity.Evaluations = new ObservableCollection<EvaluationListModel>(Activity.Evaluations); // :)
  }


  void SetMessage(string message, bool isError = false) {
    IsError = isError;
    Message = message;

    MainThread.InvokeOnMainThreadAsync(async () => {
      await Task.Delay(5000);
      ClearMessage();
    });
  }

  void ClearMessage() {
    Message = string.Empty;
    IsError = false;
  }

  public async void ApplyQueryAttributes(IDictionary<string, object> query) {
    if (Guid.TryParse(query[nameof(SubjectDetailModel.Id)].ToString(), out Guid parsedGuid)) {
      ActivityId = parsedGuid;
      await LoadDataAsync();
    } else {
      // TODO: Invalid uri parameter `Id`
      throw new InvalidDataException("Invalid uri parameter for activity page");
    }
  }

  private async Task LoadActivityAsync() {
    var activity = await activityFacade.GetAsync(ActivityId);
    Activity = activity ??
               throw new InvalidDataException($"Could not find activity with id = {ActivityId}");
    Activity.Evaluations = activity.Evaluations
      .OrderBy(e => e.StudentLogin)
      .ToObservableCollection();
  }

  private async Task<IEnumerable<HasSubjectListModel>> LoadSubjectStudents() {
    IEnumerable<HasSubjectListModel> students =
      (await hasSubjectFacade.GetSubjectStudentsAsync(Activity.SubjectId)).ToList();
    return students;
  }

  protected override async Task LoadDataAsync() {
    if (ActivityId == Guid.Empty) return;

    ClearMessage();

    await LoadActivityAsync();
    IEnumerable<HasSubjectListModel> students = (await LoadSubjectStudents()).ToList();

    // Check if all students have their evaluation created
    if (Activity.Evaluations.Count == students.Count())
      return;

    await AddMissingEvaluations(students);
    await LoadActivityAsync();
  }

  private async Task AddMissingEvaluations(IEnumerable<HasSubjectListModel> students) {
    // Assign evaluation to students who have missed their evaluation.
    foreach (var student in students) {
      if (Activity.Evaluations.Select(e => e.StudentId).Contains(student.UserId))
        continue;

      // Student does not have an evaluation
      EvaluationDetailModel missingEvaluation = EvaluationDetailModel.Empty with {
        StudentId = student.UserId,
        ActivityId = Activity.Id,
        Attended = false,
        Points = 0,
      };

      await evaluationFacade.SaveAsync(missingEvaluation);
    }

  }
}