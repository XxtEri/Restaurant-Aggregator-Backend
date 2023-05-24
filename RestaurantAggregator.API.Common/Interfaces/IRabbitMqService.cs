using RabbitMQ.Client;

namespace RestaurantAggregator.API.Common.Interfaces;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}