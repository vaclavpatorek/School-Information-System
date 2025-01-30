using Microsoft.EntityFrameworkCore;

namespace SchoolIS.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<SchoolISDbContext> dbContextFactory)
  : IUnitOfWorkFactory {
  public IUnitOfWork Create() {
    return new UnitOfWork(dbContextFactory.CreateDbContext());
  }
}