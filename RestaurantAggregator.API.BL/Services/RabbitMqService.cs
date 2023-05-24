using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Notifications.BL.Hubs;
using Notifications.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestaurantAggregator.API.BL.Configurations;
using RestaurantAggregator.API.Common.Interfaces;

namespace RestaurantAggregator.API.BL.Services;

public class RabbitMqService: IRabbitMqService
{
    private readonly RabbitMqConfiguration _configuration;

    public RabbitMqService(IConfiguration configuration)
    {
        _configuration = new RabbitMqConfiguration(
            configuration.GetSection("RabbitMqConfiguration:HostName").Get<string>(),
            configuration.GetSection("RabbitMqConfiguration:UserName").Get<string>(),
            configuration.GetSection("RabbitMqConfiguration:Password").Get<string>());
    }
    
    public IConnection CreateChannel()
    {
        var connection = new ConnectionFactory
        {
            HostName = _configuration.HostName,
            UserName = _configuration.UserName,
            Password = _configuration.Password,
            VirtualHost = "/"
        };

        var channel = connection.CreateConnection();
        return channel;
    }
}