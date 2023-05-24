using Microsoft.AspNetCore.SignalR;
using Notifications.BL.Hubs;
using Notifications.Hubs;
using RabbitMQ.Client.Events;

namespace RestaurantAggregator.API.BL.Configurations;

public class RabbitMqConfiguration
{
    public readonly string HostName;
    public readonly string UserName;
    public readonly string Password;
    
    public RabbitMqConfiguration(string hostName, string userName, string password)
    {
        HostName = hostName;
        UserName = userName;
        Password = password;
    }
    
    // public void Connect()
    // {
    //     _channel.QueueDeclare(queue: "RabbitQueue", durable: true, exclusive: false, autoDelete: false);
    //
    //     var consumer = new EventingBasicConsumer(_channel);
    //     
    //     consumer.Received += delegate
    //     {
    //         var hub = (IHubContext<NotificationHub>)_serviceProvider.GetService(typeof(IHubContext<ChatHub>))!;
    //         
    //         hub.Clients.All.SendAsync("ReceiveMessage", "You have received a message");
    //
    //     };
    //     
    //     _channel.BasicConsume(queue: "RabbitQueue", autoAck: true, consumer: consumer);
    // }
}