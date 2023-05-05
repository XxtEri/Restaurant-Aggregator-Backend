using RestaurantAggregator.AdminPanel.Common.Enums;

namespace RestaurantAggregator.AdminPanel.Models;

public class BasicResponse<T>: IBasicResponse<T>
{
    public StatusCode StatuseCode { get; set; }
    public string Description { get; set; }
    public T Data { get; }
}

public interface IBasicResponse<T>
{
    T Data { get; }
}