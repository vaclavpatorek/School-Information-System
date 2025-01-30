using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for setting selected (or disabled) color for tabbar item. 
class TabBarOpacityConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    string? selected_tab = (string?)value;
    string? wanted_tab = (string?)parameter;
    return selected_tab == wanted_tab ? 1.0 : 0.4;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
