using SchoolIS.Common.Enums;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for setting border color of activity type filter buttons.
class ActivityTypeFilterBorderConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not BitVector32 filters
      || parameter is not ActivityType type)
      return Colors.Red;

    ActivityTypeConverter fgColorConv = new();
    ActivityTypeFilterConverter bgColorConv = new();

    return filters[1 << (int)type]
      ? fgColorConv.Convert(type, typeof(Color), null, culture) 
      : bgColorConv.Convert(filters, typeof(Color), type, culture);
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
