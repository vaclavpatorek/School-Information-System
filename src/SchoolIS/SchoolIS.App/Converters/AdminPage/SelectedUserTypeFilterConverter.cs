using System.Globalization;
using SchoolIS.Common.Enums;

namespace SchoolIS.App.Converters.AdminPage;

public class SelectedUserTypeFilterConverter : IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (parameter is not UserType userType) return Colors.Red;
    UserType? activeFilter = value as UserType?;

    return userType == activeFilter ? App.StaticResource("ActiveFilterBackgroundColor") : App.StaticResource("InactiveFilterBackgroundColor");
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}