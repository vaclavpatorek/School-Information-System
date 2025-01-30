using System.Globalization;
using SchoolIS.BL.Models;

namespace SchoolIS.App.Converters.SubjectsPage;

class RowBorderColorConverter : IMultiValueConverter {
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
    if (values.Length < 2
        || values[0] is not Guid currentActivityId
        || values[1] is not ActivityListModel activity)
      return Colors.Red;

    ActivityTypeConverter conv = new();
    
    return currentActivityId == activity.Id
      ? ((Color)(conv.Convert(activity.ActivityType, typeof(Color), null, culture) ?? Colors.DarkOrange))
      : Colors.Transparent;
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}