using Microsoft.EntityFrameworkCore;
using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Seeds;

public static class SubjectSeeds {
  public static SubjectEntity Ipk = new() {
    Id = Guid.Parse("a6de8ff8-8111-4506-8138-36bdb6e04658"),
    Shortcut = "IPK",
    Name = "Computer Communications and Networks",
    Info =
      "Internet concept and model of the Internet. Model ISO/OSI. TCP, UDP, IP protocols. Addressing in computer networks (local and Internet). Routing on the Internet. Multicasting on the Internet. Switching and switches. Principles of reliable data transfer. Network interconnections. Multiple access. Error control. Communication services and protocols. Wireless and mobile networks and protocols. Network security. Programming network applications.",
  };

  public static readonly SubjectEntity Ids = new() {
    Id = Guid.Parse("ca3a1800-d342-4562-abd3-91910f4690a2"),
    Shortcut = "IDS",
    Name = "Database Systems",
    Info =
      "Fundamentals of database systems (DBS). Conceptual Modelling. The relational model. Relational database design from a conceptual model. Normalization-based design of a relational database. SQL language. Transaction processing. DBS architectures: client/server, multi-tier architectures, distributed DBS. Introduction to database administration: data security and integrity, introduction to physical database design, performance optimization, database recovery, concurrency control. Trends in database technology. The project consists in designing the structure of a relational database and programming SQL scripts to create the database, querying data in it, database triggers, and stored procedures.",
  };

  public static readonly SubjectEntity Ifj = new() {
    Id = Guid.Parse("9714834d-a828-45e1-a9a5-79047e46c778"),
    Shortcut = "IFJ",
    Name = "Formal Languages and Compilers",
    Info =
      "This course discusses formal languages and their models. Based on these models, it explains the construction of compilers. The lectures are organized as follows: (I) Basic notions: formal languages and their models, grammars, automata; compilers. (II) Regular languages and lexical analysis: regular languages and expressions, finite automata and transducers, lexical analyzer; Lex; symbol table. (III) Context-free languages and syntax analysis: context-free grammars, pushdown automata and transducers, deterministic top-down syntax analysis (recursive descent), the essence of deterministic bottom-up syntax analysis; Yacc. (IV) Semantic analysis and code generation: semantic checks, intermediate code generation, optimization, code generation.",
  };

  public static readonly SubjectEntity[] All = [Ipk, Ids, Ifj];

  static SubjectSeeds() {
    foreach (var a in ActivitySeeds.IpkActivities)
      Ipk.Activities.Add(a);
    foreach (var a in HasSubjectSeeds.EnrollsIpk)
      Ipk.Users.Add(a);
    foreach (var a in HasSubjectSeeds.TeachesIpk)
      Ifj.Users.Add(a);

    foreach (var a in ActivitySeeds.IdsActivities)
      Ids.Activities.Add(a);
    foreach (var a in HasSubjectSeeds.EnrollsIds)
      Ids.Users.Add(a);
    foreach (var a in HasSubjectSeeds.TeachesIds)
      Ifj.Users.Add(a);

    foreach (var a in ActivitySeeds.IfjActivities)
      Ifj.Activities.Add(a);
    foreach (var a in HasSubjectSeeds.EnrollsIfj)
      Ifj.Users.Add(a);
    foreach (var a in HasSubjectSeeds.TeachesIfj)
      Ifj.Users.Add(a);
  }

  public static void Seed(this ModelBuilder modelBuilder) {
    modelBuilder.Entity<SubjectEntity>()
      .HasData(
        All.Select(r => r with {
          Activities = Array.Empty<ActivityEntity>(),
          Users = Array.Empty<HasSubjectEntity>(),
        })
      );
  }
}