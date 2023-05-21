using System.Text.Json.Serialization;

namespace RestaurantAggregator.CommonFiles.Enums;

[Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
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