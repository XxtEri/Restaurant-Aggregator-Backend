using RestaurantAggregator.AuthApi.Common.DTO;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IAdminProfileService
{
    Task GetFullUserAccount(string userId);
    Task ChangeUser(string userId, ChangeUserDTO model);
    Task SetStatusBannedUser(string userId, bool isBanned);
    
    Task AppointManagerInRestaurant(string managerId, Guid restaurantId);
    Task AppointCookInRestaurant(string cookId, Guid restaurantId);

    Task RegisterUser(RegisterUserDto model);
    Task RegisterManager(string userId, Guid restaurantId);
    Task RegisterCook(string userId, Guid restaurantId);
    Task RegisterCourier(string userId);

    Task DeleteUser(string userId);
    Task DeleteManager(string managerId);
    Task DeleteCook(string cookId);
    Task DeleteCourier(string courierId);
}