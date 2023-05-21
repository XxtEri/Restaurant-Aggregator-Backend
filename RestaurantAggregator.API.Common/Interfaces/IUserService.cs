
using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IUserService
{
    Task<Guid> AddNewCustomerToDb(Guid customerId);
    string? GetUserIdFromToke(string token);
}