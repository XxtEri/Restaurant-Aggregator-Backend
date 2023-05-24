using System.Security.Claims;
using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AuthApi.Common.IServices;

public interface IAdminProfileService
{
    Task<ClaimsIdentity> LoginAdmin(LoginCredentialDto model);
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
    Task DeleteManagerRole(Guid userId);
    Task DeleteCookRole(Guid userId);
    Task DeleteCourierRole(Guid userId);
    
    Task<Guid?> GetRestaurantIdForManager(Guid userId);
    Task<Guid?> GetRestaurantIdForCook(Guid userId);
}