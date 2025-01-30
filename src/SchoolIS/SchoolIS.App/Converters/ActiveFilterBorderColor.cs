using System.Globalization;
using SchoolIS.Common.Enums;

namespace SchoolIS.App.Converters;

public class ActiveFilterBorderColor : IValueConverter {

  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (parameter is not UserType userType) return Colors.Red;
    UserType? activeFilter = value as UserType?;

    if (activeFilter != userType) return Brush.Transparent;

    switch (activeFilter) {
      case UserType.Teacher:
        return App.StaticResource("TeacherColor")!;

      case UserType.Student:
        return App.StaticResource("StudentColor")!;

      default:
        return new ArgumentException("Invalid user type");
    }
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}