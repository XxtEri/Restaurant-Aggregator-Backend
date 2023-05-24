using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RestaurantAggregator.API.Common.Interfaces;

namespace RestaurantAggregator.API.BL.Services;

public class ProducerService: IProducerService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IConfiguration _configuration;

    public ProducerService(IRabbitMqService rabbitMqService, IConfiguration configuration)
    {
        _rabbitMqService = rabbitMqService;
        _configuration = configuration;
    }

    public void SendMessage<T>(T message)
    {
        using var channel = _rabbitMqService.CreateChannel().CreateModel();

        var queue = _configuration.GetSection("MyConfiguration:QueueName").Get<string>();
        var exchange = _configuration.GetSection("MyConfiguration:ExchangeName").Get<string>();

        channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: exchange, routingKey: queue, body: body);
    }
}