using System.Globalization;

namespace SchoolIS.App.Converters.AdminPage;

public class SelectedBackground : IMultiValueConverter {
  private static object? StaticResource(string name) {
    var res = Application.Current!.Resources.TryGetValue(name, out var resource);
    return res ? resource : null;
  }

  public object? Convert(object[] value, Type targetType, object? parameter, CultureInfo culture) {
    if (value[0] is not Guid id) return Colors.DarkRed;
    
    var x = value[1]?.GetType();
    if (value[1] is not IEnumerable<Guid> list) return Colors.Transparent;

    Application.Current!.Resources.TryGetValue("SelectedCollectionItemBackground",
      out var selectedColor);

    return list.Any(obj => (Guid)obj == id) ? selectedColor : StaticResource("ApplicationBackground");
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}