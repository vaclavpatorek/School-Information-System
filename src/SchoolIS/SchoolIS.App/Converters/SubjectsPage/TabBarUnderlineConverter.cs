using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for displaying underline in selected tabbar item.
class TabBarUnderlineConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    string? selected_tab = value as string;
    string? wanted_tab = parameter as string;
    return selected_tab == wanted_tab ? TextDecorations.Underline : TextDecorations.None;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
