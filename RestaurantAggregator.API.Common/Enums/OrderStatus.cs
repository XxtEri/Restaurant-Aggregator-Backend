using System.Text.Json.Serialization;

namespace RestaurantAggregator.API.Common.Enums;

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