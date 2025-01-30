using System.Globalization;

namespace SchoolIS.App.Converters.AdminPage;

public class InverseBooleanConverter : IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    return value is not bool b ? throw new ArgumentException("Expected bool value") : !b;
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}