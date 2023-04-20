using System.Text.Json.Serialization;

namespace RestaurantAggregator.API.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Dessert,
    Drink
}