using Microsoft.EntityFrameworkCore;
using SchoolIS.BL.Facades;
using SchoolIS.BL.Facades.Interfaces;
using SchoolIS.BL.Models;
using SchoolIS.Common.Tests;
using SchoolIS.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace SchoolIS.BL.Tests;

public sealed class RoomFacadeTests : FacadeTestsBase {
  private readonly IRoomFacade _roomFacadeSUT;

  public RoomFacadeTests(ITestOutputHelper output) : base(output) {
    _roomFacadeSUT = new RoomFacade(UnitOfWorkFactory, RoomModelMapper);
  }

  [Fact]
  public async Task Create_WithNonExistingItem_DoesNotThrow() {
    // Arrange
    var model = RoomDetailModel.Empty;

    // Act & Assert
    await _roomFacadeSUT.SaveAsync(model);
  }

  [Fact]
  public async Task GetAll_Single_SeededRoom() {
    // Act
    var rooms = await _roomFacadeSUT.GetAsync();
    var room = rooms.Single(i => i.Id == RoomSeeds.Room.Id);

    // Assert
    DeepAssert.Equal(RoomModelMapper.MapToListModel(RoomSeeds.Room), room);
  }

  [Fact]
  public async Task GetById_SeededRoom() {
    // Act
    var room = await _roomFacadeSUT.GetAsync(RoomSeeds.Room.Id);

    // Assert
    DeepAssert.Equal(RoomModelMapper.MapToDetailModel(RoomSeeds.Room), room);
  }

  [Fact]
  public async Task GetById_NonExistent() {
    // Act
    var room = await _roomFacadeSUT.GetAsync(RoomSeeds.EmptyRoom.Id);

    // Assert
    Assert.Null(room);
  }

  [Fact]
  public async Task SeededRoom_DeleteById_Deleted() {
    // Arrange
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

    // Act
    await _roomFacadeSUT.DeleteAsync(RoomSeeds.RoomDelete.Id);

    // Assert
    Assert.False(await dbxAssert.RoomEntities.AnyAsync(i => i.Id == RoomSeeds.RoomDelete.Id));
  }

  [Fact]
  public async Task Delete_RoomUsedByActivity_Throws() {
    //Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      await _roomFacadeSUT.DeleteAsync(RoomSeeds.Room.Id));
  }

  [Fact]
  public async Task NewRoom_InsertOrUpdate_RoomAdded() {
    //Arrange
    var room = new RoomDetailModel {
      Id = Guid.Empty,
      Name = "UPDATED ROOM",
    };

    //Act
    room = await _roomFacadeSUT.SaveAsync(room);

    //Assert
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
    var roomFromDb = await dbxAssert.RoomEntities.SingleAsync(i => i.Id == room.Id);
    DeepAssert.Equal(room, RoomModelMapper.MapToDetailModel(roomFromDb));
  }

  [Fact]
  public async Task SeededRoom_InsertOrUpdate_RoomUpdated() {
    //Arrange
    var room = new RoomDetailModel {
      Id = RoomSeeds.Room.Id,
      Name = RoomSeeds.Room.Name,
    };
    room.Name += "updated";

    //Act
    await _roomFacadeSUT.SaveAsync(room);

    //Assert
    await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
    var roomFromDb = await dbxAssert.RoomEntities.SingleAsync(i => i.Id == room.Id);
    DeepAssert.Equal(room, RoomModelMapper.MapToDetailModel(roomFromDb));
  }
}