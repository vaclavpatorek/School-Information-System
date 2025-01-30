using SchoolIS.App.Models;
using SchoolIS.App.Services.Interfaces;
using SchoolIS.App.ViewModels;
using SchoolIS.App.Views;

namespace SchoolIS.App.Services;

public class NavigationService : INavigationService {
  public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel> {
    new("//timetable", typeof(TimetablePage), typeof(TimetableViewModel)),
    new("//subjects", typeof(SubjectPage), typeof(SubjectViewModel)),
    new("//profile", typeof(ProfilePage), typeof(ProfileViewModel)),
    new("//admin", typeof(AdminPage), typeof(AdminViewModel)),
    new("//activities", typeof(EvaluationPage), typeof(EvaluationViewModel)),
    new("//login", typeof(LoginPage), typeof(LoginViewModel)),
  };

  public async Task GoToAsync<TViewModel>()
    where TViewModel : IViewModel {
    var route = GetRouteByViewModel<TViewModel>();
    await Shell.Current.GoToAsync(route, animate: false);
  }

  public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
    where TViewModel : IViewModel {
    var route = GetRouteByViewModel<TViewModel>();
    await Shell.Current.GoToAsync(route, animate: false, parameters: parameters);
  }

  public async Task GoToAsync(string route) => await Shell.Current.GoToAsync(route, animate: false);

  public async Task GoToAsync(string route, IDictionary<string, object?> parameters) =>
    await Shell.Current.GoToAsync(route, animate: false, parameters);

  public bool SendBackButtonPressed() => Shell.Current.SendBackButtonPressed();

  private string GetRouteByViewModel<TViewModel>()
    where TViewModel : IViewModel =>
    Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
}