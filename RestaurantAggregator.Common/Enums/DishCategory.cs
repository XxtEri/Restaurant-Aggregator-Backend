using System.Text.Json.Serialization;

namespace RestaurantAggregator.CommonFiles.Enums;

[Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Dessert,
    Drink
}