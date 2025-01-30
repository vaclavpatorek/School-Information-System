using Microsoft.Extensions.DependencyInjection;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers;
using SchoolIS.DAL.UnitOfWork;

namespace SchoolIS.BL;

public static class BLInstaller {
  public static IServiceCollection AddBLServices(this IServiceCollection services) {
    services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

    services.Scan(selector => selector
      .FromAssemblyOf<BusinessLogic>()
      .AddClasses(filter => filter.AssignableTo(typeof(IFacade<,,>)))
      .AsMatchingInterface()
      .WithSingletonLifetime());

    services.Scan(selector => selector
      .FromAssemblyOf<BusinessLogic>()
      .AddClasses(filter => filter.AssignableTo(typeof(ModelMapperBase<,,>)))
      .AsMatchingInterface()
      .WithSingletonLifetime());

    return services;
  }
}