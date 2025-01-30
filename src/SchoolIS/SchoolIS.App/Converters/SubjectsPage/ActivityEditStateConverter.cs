using System.Diagnostics;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for button on the bottom of activity form.
class ActivityEditStateConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not bool editing)
      return null;

    if (targetType == typeof(string)) {
      if (parameter is not string parts)
        return "error: Invalid parameter in ActivityEditStateConverter";
      return parts.Split(';')[editing ? 1 : 0];
    }

    if (targetType == typeof(double))
      return editing ? 100 : 180;

    Trace.WriteLine($"ActivityEditStateConverter: Invalid targetType '{targetType}'");
    return null;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
