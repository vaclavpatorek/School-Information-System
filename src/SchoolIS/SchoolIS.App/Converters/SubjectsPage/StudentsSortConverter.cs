using SchoolIS.BL.Models;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;
class StudentsSortConverter : IMultiValueConverter {
  public static IEnumerable<T> Sorted<T, U>(IEnumerable<T> values, bool sortAsc, Func<T, U> predicate) {
    return sortAsc
        ? values.OrderBy(predicate)
        : values.OrderByDescending(predicate);
  }

  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
    if (values.Length < 3
      || values[0] is not IEnumerable<HasSubjectDetailModel> students
      || values[1] is not bool sortAsc
      || values[2] is not string orderBy
      || values[3] is not string loginFilter
      || values[4] is not string nameFilter)
      return Array.Empty<HasSubjectDetailModel>();

    var filteredStudents = students.AsEnumerable();

    // Filter by user login if needed.
    if (loginFilter != "")
      filteredStudents = filteredStudents.Where(
        x => x.UserLogin.Contains(loginFilter, StringComparison.CurrentCultureIgnoreCase));

    // Filter by user full name if needed.
    if (nameFilter != "")
      filteredStudents = filteredStudents.Where(
        x => $"{x.UserLastName} {x.UserFirstName}"
          .Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));

    // Order by selected filter.
    return orderBy.ToLower() switch {
      "login" => Sorted(filteredStudents, sortAsc, u => u.UserLogin),
      "name" => Sorted(filteredStudents, sortAsc, u => $"{u.UserLastName} {u.UserFirstName}"),
      "points" => Sorted(filteredStudents, sortAsc, u => u.TotalPoints),
      _ => Array.Empty<HasSubjectDetailModel>(),
    };
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
