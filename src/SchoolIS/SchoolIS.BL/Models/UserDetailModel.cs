using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using SchoolIS.Common.Enums;

namespace SchoolIS.BL.Models;

public record UserDetailModel : ModelBase {
  public string FirstName { get; set; } = String.Empty;
  public string LastName { get; set; } = String.Empty;
  public UserType Type { get; set; } = UserType.Teacher;
  public string? PhotoUrl { get; set; }
  public string? Email { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Password { get; set; }

  private string _login = String.Empty;

  public string Login {
    get {
      _login = GenerateLogin();
      return _login;
    }
    set {
      _login = value;
    }
  }

  public ObservableCollection<HasSubjectListModel> Subjects { get; init; } = [];

  public static UserDetailModel Empty =>
    new() {
      Id = Guid.NewGuid(),
      FirstName = String.Empty,
      LastName = String.Empty,
      Type = UserType.Teacher,
      PhotoUrl = null,
      Email = null,
      PhoneNumber = null,
      Password = null,
    };


  private static readonly int LoginLength = 5;

  private string GenerateLogin() {
    if (FirstName.Length + LastName.Length == 0) return "";

    string firstName = RemoveDiacritics(FirstName);
    string lastName = RemoveDiacritics(LastName);

    string lastNamePart = lastName.Length > LoginLength ? lastName[..LoginLength] : lastName;
    lastNamePart = lastNamePart.ToLower();

    // Only last name
    if (lastName.Length == LoginLength) return $"x{lastNamePart}00";

    // Last name + first name
    int remainingLength = LoginLength - lastNamePart.Length;
    string firstNamePart =
      firstName.Length > remainingLength ? firstName[..remainingLength] : firstName;

    firstNamePart = firstNamePart.ToLower();

    // Invalid xlogin
    return firstNamePart.Length < remainingLength
      ? $"x{lastNamePart}{firstNamePart}"
      : $"x{lastNamePart}{firstNamePart}00";
  }

  public static string RemoveDiacritics(string input) {
    string normalizedString = input.Normalize(NormalizationForm.FormD);
    Regex nonAsciiRegex = new("[^a-zA-Z0-9 ]");
    string withoutDiacritics = nonAsciiRegex.Replace(normalizedString, "");
    return withoutDiacritics;
  }
}