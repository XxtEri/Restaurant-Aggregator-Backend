using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notifications.Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notifications.BL.Services;

public class ReceiverService: IReceiverService, IDisposable
{
    private readonly ILogger<ReceiverService> _logger;
    private readonly IModel _model;
    private readonly IConnection _connection;
    private readonly INotificationService _notificationService;

    private static string _queueName = null!;
    
    public ReceiverService(IRabbitMqService rabbitMqService, INotificationService notificationService, ILogger<ReceiverService> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
        
        _connection = rabbitMqService.CreateChannel();

        _queueName = "MyQueue";
        
        _model = _connection.CreateModel();
        _model.QueueDeclare(
            _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false);
    }

    public async Task ReadMessage()
    {
        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await _notificationService.SendNotification(message);
            
            _logger.LogInformation("Message received from RabbitMQ: {0}", message);;

            await Task.CompletedTask;
            _model.BasicAck(ea.DeliveryTag, false);
        };
        
        _model.BasicConsume(
            _queueName, 
            false, 
            consumer);
        
        await Task.CompletedTask;
    }

    public void Dispose()
    {
       if (_model.IsOpen)
           _model.Close();
       
       if (_connection.IsOpen)
           _connection.Close();
    }
}