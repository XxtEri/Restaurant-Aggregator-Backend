using Microsoft.Extensions.Configuration;
using Notifications.Common.Interfaces;
using RabbitMQ.Client;

namespace Notifications.BL.Services;

public class RabbitMqService: IRabbitMqService
{
    //private readonly RabbitMqConfiguration _configuration;

    public RabbitMqService(IConfiguration configuration)
    {
        
    }
    
    public IConnection CreateChannel()
    {
        var connection = new ConnectionFactory
        {

        };

        var channel = connection.CreateConnection();
        return channel;
    }
}