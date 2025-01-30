using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Seeds;
public static class UserSeeds {
  public static List<UserEntity> Users { get; } = GetUsers();

  public static List<UserEntity> Teachers => Users.Where(u => u.UserType == UserType.Teacher).ToList();
  public static List<UserEntity> Students => Users.Where(u => u.UserType == UserType.Student).ToList();

  private static List<UserEntity> GetUsers() {
    var data = new object?[,] {
      { "f159f7fc-7daf-492e-bc25-2f316fe0d00b", UserType.Teacher, "Jana", "Nováková", "jana_novakova.jpg", "xjananov00", "jana.novakova@example.com", "+123456789" },
      { "4d7eaf40-4281-4ef5-99eb-206a18f38cb5", UserType.Teacher, "Petr", "Svoboda", "petr_svoboda.jpg", "xpetrsv00", null, "+987654321" },
      { "f14f4a77-c389-4dd5-99f5-789d1a901d64", UserType.Teacher, "Eva", "Králová", null, "xevakra00", "eva.kralova@example.com", null },
      { "aef39f15-aa07-4e64-b819-c8a8e5fa91f9", UserType.Student, "Jan", "Novák", "jan_novak.jpg", "xjannov00", "jan.novak@example.com", "+111222333" },
      { "9c8eb72a-94b8-4cc0-855b-bb9ee8ebe037", UserType.Student, "Marie", "Černá", "marie_cerna.jpg", "xmaricer00", null, null },
      { "28d865d9-37ff-4df5-9b5e-019b274df1d2", UserType.Student, "Jiří", "Procházka", "jiri_prochazka.jpg", "xjiripr00", "jiri.prochazka@example.com", "+555444777" },
      { "548b2c98-bb7b-4e9b-9d85-ff7d4d5d5c6e", UserType.Student, "Martin", "Kučera", "martin_kucera.jpg", "xmartiku00", null, "+999888777" },
      { "e38d6d8e-f5c1-4c81-b454-04994aa1a4f5", UserType.Student, "Lucie", "Malá", "lucie_mala.jpg", "xlucimal00", "lucie.mala@example.com", null },
      { "cf0b5b8e-8f12-4d39-83f4-4d89f9cfbb7d", UserType.Student, "Jiřina", "Veselá", "jirina_vesela.jpg", "xjirives00", null, "+333222111" },
      { "a53b4d1f-0578-4ba3-9d08-53a9f46b13b0", UserType.Student, "Tomáš", "Šťastný", "tomas_stastny.jpg", "xtomstas00", "tomas.stastny@example.com", null },
      { "e10c0d8c-9ad8-4b48-814a-f60d7a5e6127", UserType.Student, "Zdeněk", "Pokorný", "zdenek_pokorny.jpg", "xzdenpok00", "zdenek.pokorny@example.com", "+777888999" },
      { "3d38e29b-aa55-4a76-8e92-7f8aa1475d50", UserType.Student, "Alena", "Horáková", "alena_horakova.jpg", "xalenaho00", null, null },
      { "e2db00ae-0ec5-48b4-a2e1-8623ff8c5a4d", UserType.Student, "František", "Veselý", "frantisek_vesely.jpg", "xfranves00", "frantisek.vesely@example.com", "+111111111" },
      { "31d9d2ff-69f3-4a5f-8f71-d878ae44dd97", UserType.Student, "Lenka", "Kovářová", "lenka_kovarova.jpg", "xlenkov00", "lenka.kovarova@example.com", null },
      { "f53b51bf-4f05-46ee-8a06-ec124b25a25c", UserType.Student, "Kateřina", "Marešová", "katerina_maresova.jpg", "xkatemar00", "katerina.maresova@example.com", null },
      { "4a8d9b08-2166-48b5-a57e-b85d3f77a675", UserType.Student, "Pavel", "Němec", "pavel_nemec.jpg", "xpavelne00", null, "+222333444" },
      { "2e3df33c-f9a3-4f79-af34-620a2eb8bb6c", UserType.Student, "Martina", "Dvořáková", "martina_dvorakova.jpg", "xmartidv00", "martina.dvorakova@example.com", null },
      { "6c09ee08-c053-4c15-850e-d824169b0f27", UserType.Student, "Michaela", "Benešová", "michaela_benesova.jpg", "xmicbene00", null, "+444555666" },
      { "d4a95656-0055-4a27-8fc1-67e30db77f57", UserType.Student, "Jakub", "Holub", "jakub_holub.jpg", "xjakuhol00", "jakub.holub@example.com", null },
      { "e47e8793-64f7-479c-bbbb-4a6dcf3bcdd7", UserType.Student, "Veronika", "Urbanová", "veronika_urbanova.jpg", "xverourb00", null, "+666777888" },
      { "c1b99ec5-f1e1-4a67-9735-17fb3be4e9e3", UserType.Student, "Josef", "Vaněk", "josef_vanek.jpg", "xjosevan00", "josef.vanek@example.com", null },
      { "5c39805c-2869-4891-91ae-686f49e7ac12", UserType.Student, "Petra", "Čermáková", "petra_cermakova.jpg", "xpetrcer00", null, "+888999000" },
      { "07f5c07b-76e7-4f48-988b-59d2068e4f47", UserType.Student, "Jan", "Krejčí", "jan_krejci.jpg", "xjankre00", "jan.krejci@example.com", null },
      { "60a65d9d-5245-4af5-89a1-0fb23b97d03e", UserType.Student, "Roman", "Fiala", "roman_fiala.jpg", "xromafia00", "roman.fiala@example.com", "+123123123" },
      { "b15cb1f8-6f63-4f16-a0fd-0b8d2e187c39", UserType.Student, "Miroslav", "Richter", "miroslav_richter.jpg", "xmirorich00", "miroslav.richter@example.com", null },
      { "1f13df06-c8cc-4c0e-91fb-1a6c99e80168", UserType.Student, "Milan", "Adam", "milan_adam.jpg", "xmiladam00", null, "+456456456" },
      { "862bc0f1-b86b-45bb-97de-5fcb490f7d36", UserType.Student, "Ivana", "Štěpánová", "ivana_stepanova.jpg", "xivanste00", null, null },
      { "e775dab0-2a13-4d7e-b9d1-b8c27363288a", UserType.Student, "Monika", "Novotná", "monika_novotna.jpg", "xmoninov00", "monika.novotna@example.com", "+987654321" },
      { "584f9947-1b33-4b77-9e1f-c7f5e12c1197", UserType.Student, "Ondřej", "Pospíšil", "ondrej_pospisil.jpg", "xondrpos00", null, "+321321321" }
    };

    List<UserEntity> users = [];

    for (int i = 0; i < data.GetLength(0); i++) {
      users.Add(new() {
        Id = Guid.Parse((string)data[i, 0]!),
        UserType = (UserType)data[i, 1]!,
        FirstName = (string)data[i, 2]!,
        LastName = (string)data[i, 3]!,
        PhotoUrl = (string?)data[i, 4],
        Login = (string)data[i, 5]!,
        Email = (string?)data[i, 6],
        PhoneNumber = (string?)data[i, 7],
        Activities = null!,
        Evaluations = null!,
        Subjects = null!,
      });
    }

    return users;
  }

  static UserSeeds() {

  }
  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<UserEntity>()
      .HasData(
        Users.Select(u => u with {
          Activities = Array.Empty<ActivityEntity>(),
          Evaluations = Array.Empty<EvaluationEntity>(),
          Subjects = Array.Empty<HasSubjectEntity>(),
        })
      );
  }
}
