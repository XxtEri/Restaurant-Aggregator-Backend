using System.Text.Json.Serialization;

namespace RestaurantAggregator.CommonFiles.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Dessert,
    Drink
}