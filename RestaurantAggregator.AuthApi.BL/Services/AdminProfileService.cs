using RestaurantAggregator.AuthApi.Common.DTO;
using RestaurantAggregator.AuthApi.Common.IServices;

namespace RestaurantAggregator.AuthApi.BL.Services;

public class AdminProfileService: IAdminProfileService
{
    public Task GetFullUserAccount(string userId)
    {
        throw new NotImplementedException();
    }

    public Task ChangeUser(string userId, ChangeUserDTO model)
    {
        throw new NotImplementedException();
    }

    public Task SetStatusBannedUser(string userId, bool isBanned)
    {
        throw new NotImplementedException();
    }

    public Task AppointManagerInRestaurant(string managerId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task AppointCookInRestaurant(string cookId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task RegisterUser(RegisterUserDto model)
    {
        throw new NotImplementedException();
    }

    public Task RegisterManager(string userId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task RegisterCook(string userId, Guid restaurantId)
    {
        throw new NotImplementedException();
    }

    public Task RegisterCourier(string userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteManager(string managerId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCook(string cookId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCourier(string courierId)
    {
        throw new NotImplementedException();
    }
}