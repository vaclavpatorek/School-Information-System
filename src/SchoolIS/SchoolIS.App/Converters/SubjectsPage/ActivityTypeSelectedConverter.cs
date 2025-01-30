using SchoolIS.Common.Enums;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converted selection of ActivityType in new activity form
class ActivityTypeSelectedConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not ActivityType actualType || parameter is not ActivityType wantedType)
      return 0.0;

    if (targetType == typeof(FontAttributes))
      return wantedType == actualType ? FontAttributes.Bold : FontAttributes.None;
    if (targetType == typeof(double))
      return wantedType == actualType ? 1.0 : 0.5;

    throw new NotImplementedException($"Invalid target type {targetType} passed to ActivityTypeSelectedConverter.");
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
