using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.AdminPanel.Common.Interfaces;

public interface IBaseCrudService<T>
{
    bool Create(T entity);
    Task<T> Get(Guid id);
    Task<List<T>> Select();
    bool Update(Guid id);
    bool Delete(Guid id);
}