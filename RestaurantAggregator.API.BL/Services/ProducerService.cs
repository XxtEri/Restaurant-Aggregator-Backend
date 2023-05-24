using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RestaurantAggregator.API.Common.Interfaces;

namespace RestaurantAggregator.API.BL.Services;

public class ProducerService: IProducerService
{
    private readonly IRabbitMqService _rabbitMqService;

    public ProducerService(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    public void SendMessage<T>(T message)
    {
        using var channel = _rabbitMqService.CreateChannel().CreateModel();
        
        channel.QueueDeclare("MyQueue", durable: false, exclusive: false, autoDelete: false);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: "", 
            routingKey: "MyQueue",
            body: body);
    }
}