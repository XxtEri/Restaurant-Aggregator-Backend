using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IAdminUsersServices
{
    Task<List<UserDto>> Select();
    Task<UserDto> Get(Guid id);
    Task Create(RegisterUserCredentialDto model);
    Task Delete(Guid id);
    Task ChangeStatusBannedUser(Guid userId);
    Task ChangeInfoUserProfile(Guid userId, UpdateInfoUserProfileDto model);
    Task AddManagerRole(Guid id);
    Task AddCookRole(Guid id);
    Task AddCourierRole(Guid id);
    Task DeleteManagerRole(Guid userId);
    Task DeleteCookRole(Guid userId);
    Task DeleteCourierRole(Guid userId);
    Task<Guid?> GetRestaurantIdForManager(Guid userId);
    Task<Guid?> GetRestaurantIdForCook(Guid userId);
    Task AppointManagerInRestaurant(Guid managerId, Guid restaurantId);
    Task AppointCookInRestaurant(Guid cookId, Guid restaurantId);
}