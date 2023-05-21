
using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IUserService
{
    Task AddNewCustomerToDb(Guid customerId);
    Task<string?> GetUserIdFromToke(string token);
}