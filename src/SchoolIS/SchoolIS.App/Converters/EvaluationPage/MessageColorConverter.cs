using System.Globalization;

namespace SchoolIS.App.Converters.EvaluationPage;

class MessageColorConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not bool isError)
      return Colors.White;

    return isError ? App.StaticResource("ErrorColor") : App.StaticResource("SuccessColor");
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo c) {
    throw new NotImplementedException();
  }
}