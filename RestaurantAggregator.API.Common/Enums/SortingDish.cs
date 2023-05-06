using System.Text.Json.Serialization;

namespace RestaurantAggregator.API.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortingDish
{
    NameAsk,
    NameDesk,
    PriceAsk,
    PriceDesk,
    RatingAsk,
    RatingDesk
}
