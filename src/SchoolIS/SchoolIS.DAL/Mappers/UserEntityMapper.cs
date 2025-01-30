using SchoolIS.DAL.Entities;

namespace SchoolIS.DAL.Mappers;

public class UserEntityMapper : IEntityMapper<UserEntity> {
  public void MapToExistingEntity(UserEntity existingEntity, UserEntity newEntity) {
    existingEntity.Email = newEntity.Email;
    existingEntity.FirstName = newEntity.FirstName;
    existingEntity.LastName = newEntity.LastName;
    existingEntity.Login = newEntity.Login;
    existingEntity.PhoneNumber = newEntity.PhoneNumber;
    existingEntity.PhotoUrl = newEntity.PhotoUrl;
    existingEntity.Password = newEntity.Password;
  }
}