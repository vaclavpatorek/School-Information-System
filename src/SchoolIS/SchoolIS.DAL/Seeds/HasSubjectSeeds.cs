using Microsoft.EntityFrameworkCore;
using SchoolIS.Common.Enums;
using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Seeds;

public static class HasSubjectSeeds {
  private static readonly string[] StudentGuids = [
    "43ed9bd4-de29-4b67-aa4e-ca6b4a5eed5b",
    "cb9a0f8d-8c48-4e26-88db-40a767b22c14",
    "0d09fbd9-ff42-41c3-b4d2-4ab5112d4d10",
    "a5c2b452-14c9-4469-8fa7-012ad3962d8b",
    "b73824e1-b1bf-4681-9da7-5f6cf63c67dd",
    "3c15573d-50c4-4a63-8aeb-6f1f259a7607",
    "bf8a3b9e-48d8-4b62-94e8-893f362764c0",
    "6981a475-b09a-4a0b-9f57-aa58c57ec5c7",
    "dd8b9f4d-0b7c-45b0-bb72-34a5a2d2e48b",
    "7e30de53-f0aa-4b3e-8d8e-33df8911f3f2",
    "4e23318e-73a5-4a7b-9fe0-1b5686a8c96e",
    "ed1107c1-900f-4e9d-bd47-bd493fd2c8db",
    "88d4a3f0-161f-4dcb-b19c-44af5ef3d452",
    "0d79ff41-9ff5-4ff0-826b-39821e335e97",
    "1f29818b-7a34-4b29-933d-934f0d4fbc10",
    "54f5d30e-1664-4456-8b7e-72551791b261",
    "f5098b79-0309-45c3-880f-7e390ca4a9b8",
    "3637b1cf-44ab-4fa8-9dcf-f7a6d59665b3",
    "f1d5aa6e-9ba1-48b0-8967-aa6e9a29544a",
    "5f17db4d-c1ef-45a5-b9dc-8d5b6482e080",
    "18138b23-32a2-4910-986f-b5f70de42c29",
    "77efbb5d-66af-4ad4-859f-32c4216f3efb",
    "c2078103-3e67-46c5-af51-b76fb3f6a5aa",
    "784f4c7f-7696-42fd-b2cb-14e7b7f02f7b",
    "0a8bc6e4-08c9-4292-a805-25a2d02fe93b",
    "d08c6ed0-aa6a-46d6-8a53-7e42e58edf49",
    "3baf2c11-60dc-42ad-b8f4-3f99d74a80fb",
    "1683889e-0731-47c6-bd26-44dbde962e0d",
    "6d394d0d-8a91-4a4f-b0ff-0203b0da02d8",
    "b589d0f6-ba6d-4e57-b25b-33e87ac256d1"
  ];

  private static readonly string[] TeacherGuids = [
    "13b06f01-c1d9-423e-a776-d04e0cb2e946",
    "fe228933-f066-44d5-9b7b-dfd90a625c4d",
    "50e1c148-fae1-4349-9b71-f69bb478bf4d",
    "4e19887a-66b2-4faa-8c8d-117a5a080261",
    "a83c55c9-02e5-4a9e-a377-91b5b6dd45c5",
    "ddf3cafd-aee5-4987-92c2-c6b9ec76eac9",
    "50eb204f-3943-46bf-b364-f7abe4a85b0d",
    "4e59313a-9c09-470a-9090-5b1126b3f1c2"
  ];

  public static readonly List<HasSubjectEntity> All = [];

  public static IEnumerable<HasSubjectEntity> TeachesIpk =>
    All.Where(e => IsSubjectTeacher(e, SubjectSeeds.Ipk.Id));

  public static IEnumerable<HasSubjectEntity> TeachesIds =>
    All.Where(e => IsSubjectTeacher(e, SubjectSeeds.Ids.Id));

  public static IEnumerable<HasSubjectEntity> TeachesIfj =>
    All.Where(e => IsSubjectTeacher(e, SubjectSeeds.Ifj.Id));


  public static IEnumerable<HasSubjectEntity> EnrollsIpk =>
    All.Where(e => IsSubjectStudent(e, SubjectSeeds.Ipk.Id));


  public static IEnumerable<HasSubjectEntity> EnrollsIds =>
    All.Where(e => IsSubjectStudent(e, SubjectSeeds.Ids.Id));

  public static IEnumerable<HasSubjectEntity> EnrollsIfj =>
    All.Where(e => IsSubjectStudent(e, SubjectSeeds.Ifj.Id));


  private static bool IsSubjectStudent(HasSubjectEntity hasSubject, Guid subjectId) {
    return hasSubject.Subject.Id == subjectId && hasSubject.User.UserType == UserType.Student;
  }

  private static bool IsSubjectTeacher(HasSubjectEntity hasSubject, Guid subjectId) {
    return hasSubject.Subject.Id == subjectId && hasSubject.User.UserType == UserType.Teacher;
  }

  static HasSubjectSeeds() {
    All.AddRange(UserSeeds.Students
      .Select((student, index) => new HasSubjectEntity {
        Id = Guid.Parse(StudentGuids[index]),
        UserId = student.Id,
        User = student,
        SubjectId = SubjectSeeds.All[index % SubjectSeeds.All.Length].Id,
        Subject = SubjectSeeds.All[index % SubjectSeeds.All.Length],
      })
    );

    All.AddRange(UserSeeds.Teachers
      .Select((teacher, index) => new HasSubjectEntity {
        Id = Guid.Parse(TeacherGuids[index]),
        SubjectId = SubjectSeeds.All[index % SubjectSeeds.All.Length].Id,
        Subject = SubjectSeeds.All[index % SubjectSeeds.All.Length],
        UserId = teacher.Id,
        User = teacher
      })
    );
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<HasSubjectEntity>().HasData(
      All.Select(e => e with {
        User = null!,
        Subject = null!,
      })
    );
  }
}