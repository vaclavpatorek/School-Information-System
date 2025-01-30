using System.Globalization;

namespace SchoolIS.App.Converters;

public class ProfileImageConverter : IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    string? photo = value as string;

    return photo ?? "user.png";
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}