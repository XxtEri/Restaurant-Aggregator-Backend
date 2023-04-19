using System.Text.Json.Serialization;

namespace RestaurantAggregatorService.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Created,
    Kitchen,
    Packaging,
    WaitingCourier,
    Delivery,
    Delivered,
    Cancelled
}