using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RestaurantAggregator.API.Common.Interfaces;

namespace RestaurantAggregator.API.BL.Services;

public class RabbitMqService: IRabbitMqService
{
    private readonly IConfiguration _configuration;

    public RabbitMqService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IConnection CreateChannel()
    {
        var connection = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"],
            UserName = _configuration["RabbitMQ:UserName"],
            Password = _configuration["RabbitMQ:Password"],
            VirtualHost = "/"
        };

        var channel = connection.CreateConnection();
        return channel;
    }
}