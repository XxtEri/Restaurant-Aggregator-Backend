using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IAdminUsersServices
{
    Task<List<UserDto>> Get();
    Task Create(RegisterUserCredentialDto model);
    Task Delete(Guid id);
}