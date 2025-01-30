using SchoolIS.BL.Models;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for setting selected background in Room selection (new activity form).
class RoomSelectorColorConverter : IMultiValueConverter {
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
    if (values.Length != 2
      || values[0] is not RoomListModel selectedRoom
      || values[1] is not RoomListModel thisRoom)
      return Colors.Red;

    return selectedRoom.Id == thisRoom.Id ? Color.FromArgb("#3F3F3F") : Color.FromArgb("#1E1E1E");
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
