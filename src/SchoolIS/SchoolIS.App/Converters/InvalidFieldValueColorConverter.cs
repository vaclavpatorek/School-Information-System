using System.Globalization;

namespace SchoolIS.App.Converters;

public class InvalidFieldValueColorConverter : IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not IEnumerable<object> list || parameter is not string field) return Colors.Red;

    return list.Contains(field) ? Colors.DarkRed : Colors.Transparent;
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}