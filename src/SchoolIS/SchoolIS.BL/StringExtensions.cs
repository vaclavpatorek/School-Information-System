namespace SchoolIS.BL;

public static class StringExtensions {
  public static string Hash(this string password) {
    // Generate a salt
    string salt = BCrypt.Net.BCrypt.GenerateSalt();

    // Hash the password using the salt
    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

    return hashedPassword;
  }
}