using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.AdminPanel.Models;

public class RestaurantListViewModel
{
    public List<RestaurantDTO> Restaurants { get; set; } = new List<RestaurantDTO>();
    public string? Name { get; set; }
}