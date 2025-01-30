using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for displaying wanted tab in tabbar.
class TabBarContentConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    string? selected_tab = (string?)value;
    string? wanted_tab = (string?)parameter;
    return selected_tab == wanted_tab ? true : false;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
