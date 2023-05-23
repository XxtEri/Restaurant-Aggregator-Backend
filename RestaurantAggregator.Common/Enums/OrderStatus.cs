using System.Text.Json.Serialization;

namespace RestaurantAggregator.CommonFiles.Enums;

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