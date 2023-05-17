using RestaurantAggregator.CommonFiles.Dto;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IAdminUsersServices
{
    Task<List<UserDto>> Select();
    Task<UserDto> Get(Guid id);
    Task Create(RegisterUserCredentialDto model);
    Task Delete(Guid id);
}