using RestaurantAggregator.API.Common.DTO;

namespace RestaurantAggregator.AdminPanel.Models;

public class RestaurantListViewModel
{
    public List<RestaurantDTO> Restaurants { get; set; }
    public string? Name { get; set; }
    public PageViewModel PageViewModel { get; }
    public RestaurantListViewModel(List<RestaurantDTO> restaurants, PageViewModel viewModel, string? name)
    {
        Restaurants = restaurants;
        PageViewModel = viewModel;
        Name = name;
    }
}