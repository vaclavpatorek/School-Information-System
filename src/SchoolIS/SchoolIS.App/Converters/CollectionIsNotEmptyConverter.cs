using System.Globalization;

namespace SchoolIS.App.Converters;

public class CollectionIsNotEmptyConverter : IValueConverter {
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not ICollection<string> num)
      return false;

    return num.Count != 0;
  }

  public object ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}