using System.Globalization;

namespace SchoolIS.App.Converters;
class SortButtonConverter : IMultiValueConverter {
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
    if (values.Length < 3
      || values[0] is not bool sortAsc
      || values[1] is not string textAsc
      || values[2] is not string textDesc)
      return "error";
    return sortAsc ? textAsc : textDesc;
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
