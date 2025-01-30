using SchoolIS.Common.Enums;
using System.Globalization;

namespace SchoolIS.App.Converters.SubjectsPage;

// Converter used for converting ActivityTType into color.
class ActivityTypeConverter : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value is not ActivityType type)
      return Colors.Red;

    return type switch {
      ActivityType.Test => App.StaticResource("TestColor"),
      ActivityType.Lecture => App.StaticResource("LectureColor"),
      ActivityType.Exercise => App.StaticResource("ExerciseColor"),
      ActivityType.Meeting => App.StaticResource("MeetingColor"),
      _ => Colors.DarkRed,
    };
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
