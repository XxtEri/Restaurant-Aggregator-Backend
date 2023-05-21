using System.ComponentModel;
using RestaurantAggregator.API.Common.DTO;
using RestaurantAggregator.API.Common.Enums;
using RestaurantAggregator.CommonFiles.Enums;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IDishService
{
    Task<DishPagedListDTO> GetListAllDishesInRestaurant(Guid restaurantId, 
        List<DishCategory> categories, 
        bool vegetarian, 
        SortingDish sorting, 
        int page);
    Task<DishPagedListDTO> GetListDishesInMenu(Guid restaurantId, 
        Guid menuId,
        List<DishCategory> categories, 
        bool vegetarian,
        SortingDish sorting,
        int page);
    Task<DishDTO> GetDishInformation(Guid dishId);
    Task<bool> CheckCurrentUserSetRatingToDish(Guid userId, Guid dishId);
    Task SetRatingToDish(Guid userId, Guid dishId, int ratingScore);
    Task<DishDTO> AddDishToMenuOfRestaurant(Guid restaurantId, Guid menuId, CreateDishDto model);
}