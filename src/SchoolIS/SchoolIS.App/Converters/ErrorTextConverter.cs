using System.Globalization;

namespace SchoolIS.App.Converters;

public class ErrorTextConverter : IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not IEnumerable<object> list || parameter is not string field) throw new ArgumentException($"Invalid parameters for {nameof(ErrorTextConverter)}");

    return list.Contains(field) ? App.StaticResource("ErrorColor"): App.StaticResource("SecondaryTextColor");
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}