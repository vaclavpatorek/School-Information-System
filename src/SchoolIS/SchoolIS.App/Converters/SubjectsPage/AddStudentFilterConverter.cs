using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using SchoolIS.BL.Models;

namespace SchoolIS.App.Converters.SubjectsPage;

class AddStudentFilterConverter : IMultiValueConverter {
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
    if (values.Length < 2
        || values[0] is not ObservableCollection<UserListModel> otherStudents
        || values[1] is not string filter)
      return Array.Empty<UserListModel>();

    return otherStudents.Where(u =>
      $"{u.FirstName}{u.LastName}".Contains(filter.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase) ||
      $"{u.Login}".Contains(filter, StringComparison.CurrentCultureIgnoreCase)).ToList();
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
    CultureInfo culture) {
    throw new NotImplementedException();
  }
}