using SchoolIS.App.Models;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for getting sort asc or sort desc icon based on boolean (true for asc).
class SortIconConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not bool asc)
      return FaSolidIcons.Cross;
    return asc ? FaSolidIcons.SortUp : FaSolidIcons.SortDown;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
