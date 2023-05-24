namespace RestaurantAggregator.API.Common.Interfaces;

public interface IProducerService
{
    void SendMessage<T>(T message);
}