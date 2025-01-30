using System.Globalization;

namespace SchoolIS.App.Converters;

public class OpacityConverter : IMultiValueConverter {
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
    if (values is not [not null, not null]) return 0.5;

    string expected = values[0].ToString()!;
    string received = values[1].ToString()!;

    return expected == received ? 1.0 : 0.5;
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}