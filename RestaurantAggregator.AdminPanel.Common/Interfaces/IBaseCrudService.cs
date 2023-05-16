using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IBaseCrudService<T>
{
    Task<T> Get(Guid id);
    Task<List<T>> Select();
    Task Create(T model);
    Task Update(Guid id, T model);
    Task Delete(Guid id);
}