using System.Collections.ObjectModel;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter for setting color of the Label in new activity form. The label should have ErrorColor
// on error and White otherwise.
// NOTE: Admins page invalid field converter cannot be used because we need to return ErrorColor (not DarkRed)
//       and White (not transparent).
class InvalidFieldLabelColor : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not ObservableCollection<string> errors
        || parameter is not string error)
      return Colors.DarkOrange;

    return errors.SingleOrDefault(e =>
      e.Equals(error, StringComparison.CurrentCultureIgnoreCase)) != null
      ? App.StaticResource("ErrorColor")
      : Colors.White;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}