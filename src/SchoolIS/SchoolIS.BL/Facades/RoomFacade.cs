using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Mappers.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.DAL.Entities;
using SchoolIS.DAL.Mappers;
using SchoolIS.DAL.UnitOfWork;

namespace SchoolIS.BL.Facades;

public class RoomFacade(
  IUnitOfWorkFactory unitOfWorkFactory,
  IRoomModelMapper roomModelMapper) :
  FacadeBase<RoomEntity, RoomListModel, RoomDetailModel, RoomEntityMapper>(unitOfWorkFactory,
    roomModelMapper), IRoomFacade {
  protected override ICollection<string> IncludesNavigationPathDetail =>
    new[] { $"{nameof(RoomEntity.Activities)}" };
}