
using System.Net;
using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IUserService
{
    Task<Guid> AddNewCustomerToDb(Guid customerId);
    Task AddNewCookToDb(Guid cookId);
    Task AddNewManagerToDb(Guid managerId);
    Task AddNewCourierToDb(Guid courierId);
    
    Task DeleteCookFromDb(Guid cookId);
    Task DeleteManagerFromDb(Guid managerId);
    Task DeleteCourierFromDb(Guid courierId);
    
    string? GetUserIdFromToken(string token);

    Task AddRestaurantIdForCook(Guid cookId, Guid restaurantId);
    Task AddRestaurantIdForManager(Guid managerId, Guid restaurantId);
}