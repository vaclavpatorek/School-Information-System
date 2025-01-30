using System.Globalization;

namespace SchoolIS.App.Converters.AdminPage;

public class BoolToStringConverter : IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not bool val || parameter is not string str) return "Error";

    string[] texts = str.Split(';');

    if (texts.Length != 2) return "Invalid number of parameters";

    return val ? texts[0] : texts[1];
  }

  public object ConvertBack(object? value, Type targetTypes, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}