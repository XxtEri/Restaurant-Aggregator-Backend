using System.Text.Json.Serialization;

namespace RestaurantAggregatorService.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Dessert,
    Drink
}