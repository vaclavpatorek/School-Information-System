using System.Globalization;

namespace SchoolIS.App.Converters.AdminPage;

public class ValidLoginConverter : IValueConverter {
  private const int LoginLength = 6;

  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not string s) return Colors.White;

    return s.Length >= LoginLength ? Colors.White : App.StaticResource("ErrorColor");
  }

  public object ConvertBack(object? value, Type targetTypes, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}