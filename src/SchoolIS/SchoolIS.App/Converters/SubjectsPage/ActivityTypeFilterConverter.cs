using SchoolIS.Common.Enums;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converted used for background color of selected activity type filters.
class ActivityTypeFilterConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not BitVector32 filters
      || parameter is not ActivityType type)
      return Colors.Red;

    if (targetType == typeof(Color))
      return filters[1 << (int)type]? Color.FromArgb("#0B0B0B") : Color.FromArgb("#212121");

    Trace.WriteLine($"ActivityTypeFilterConverter: Invalid target type: {targetType}");
    return Colors.Red;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
