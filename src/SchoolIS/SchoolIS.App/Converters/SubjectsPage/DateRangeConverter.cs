using SchoolIS.BL.Models;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for getting first and last date of Activities.
class DateRangeConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not IEnumerable<ActivityListModel> activities) {
      return $"error: DateRangeConverter: value type '{value?.GetType()}' is not supported.";
    }

    IEnumerable<ActivityListModel> activityListModels = activities as ActivityListModel[] ?? activities.ToArray();
    if (activityListModels.FirstOrDefault() == null) {
      return "error: DateRangeConverter: activities are empty";
    }

    var dateMin = activityListModels.First().StartsFrom;
    var dateMax = activityListModels.First().StartsFrom;

    foreach (var a in activityListModels) { 
      if (a.StartsFrom < dateMin)
        dateMin = a.StartsFrom;
      if (a.StartsFrom >  dateMax)
        dateMax = a.StartsFrom;
    }

    return $"{dateMin:MM}.{dateMin:dd}. - {dateMax:MM}.{dateMax:dd}.";
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
