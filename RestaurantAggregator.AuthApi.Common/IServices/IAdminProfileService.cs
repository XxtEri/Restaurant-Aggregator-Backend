using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IAdminProfileService
{
    Task<List<UserDto>> GetUsers();
    Task<UserDto> GetUser(Guid userId);
    Task ChangeUser(Guid userId, UpdateInfoUserProfileDto model);
    Task ChangeStatusBannedUser(Guid userId);
    
    Task AppointManagerInRestaurant(Guid managerId, Guid restaurantId);
    Task AppointCookInRestaurant(Guid cookId, Guid restaurantId);

    Task RegisterUser(RegisterUserCredentialDto model);
    Task RegisterManager(Guid userId);
    Task RegisterCook(Guid userId);
    Task RegisterCourier(Guid userId);

    Task DeleteUser(Guid userId);
    Task<Guid?> GetRestaurantIdForManager(Guid userId);
    Task<Guid> GetRestaurantIdForCook(Guid userId);
}