using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter that returns true if the string is empty.
class StringIsNotEmptyConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is string)
      return (string)value != "";
    return false;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
