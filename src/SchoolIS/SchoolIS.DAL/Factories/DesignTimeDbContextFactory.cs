using Microsoft.EntityFrameworkCore.Design;

namespace SchoolIS.DAL.Factories;

/// <summary>
///   EF Core CLI migration generation uses this DbContext to create model and migration
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SchoolISDbContext> {
  private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new("SchoolIS");

  public SchoolISDbContext CreateDbContext(string[] args) {
    return _dbContextSqLiteFactory.CreateDbContext();
  }
}