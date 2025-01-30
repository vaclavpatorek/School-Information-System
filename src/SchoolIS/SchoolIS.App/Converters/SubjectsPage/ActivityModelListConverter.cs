using SchoolIS.BL.Models;
using System.Collections.Specialized;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for filtering of activities table
class ActivityModelListConverter : IMultiValueConverter {
  private static IEnumerable<ActivityListModel> OrderBy(IEnumerable<ActivityListModel> a, bool asc) {
    return asc ? a.OrderBy(a => a.StartsFrom) : a.OrderByDescending(a => a.StartsFrom);
  }

  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
    if (values.Length < 3
      || values[0] is not IEnumerable<ActivityListModel> activities
      || values[1] is not BitVector32 filters
      || values[2] is not bool sortAsc)
      return Array.Empty<ActivityListModel>();

    if (filters.Data == 0) {
      return OrderBy(activities, sortAsc);
    } else {
      return OrderBy(activities
        .Where(a => filters[1 << (int)a.ActivityType]), sortAsc);
    }
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}

