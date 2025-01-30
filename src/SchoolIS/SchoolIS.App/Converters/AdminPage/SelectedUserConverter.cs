using System.Globalization;
using SchoolIS.Common.Enums;

namespace SchoolIS.App.Converters.AdminPage;

public class SelectedUserConverter : IValueConverter {
  private static readonly Dictionary<UserType, object> TypeColors = new() {
    { UserType.Student, App.StaticResource("StudentColor") },
    { UserType.Teacher, App.StaticResource("TeacherColor") }
  };

  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not UserType userType) return Colors.Red;
    if (parameter is null) return TypeColors[userType];
    if (parameter is not UserType expectedUserType) return Colors.Red;
    return userType == expectedUserType ? TypeColors[userType] : App.StaticResource("SecondaryTextColor");
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}