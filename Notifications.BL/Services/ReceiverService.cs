using System.Text;
using Microsoft.Extensions.Configuration;
using Notifications.Common.Dto;
using Notifications.Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notifications.BL.Services;

public class ReceiverService: IReceiverService, IDisposable
{
    private readonly IModel _model;
    private readonly IConnection _connection;
    private readonly INotificationService _notificationService;

    private static string _queueName = null!;

    public ReceiverService(IConfiguration configuration, INotificationService notificationService, IRabbitMqService rabbitMqService)
    {
        _notificationService = notificationService;
        //_queueName = configuration.GetSection("MyConfiguration:QueueName").Get<string>();
        _connection = rabbitMqService.CreateChannel();
        _model = _connection.CreateModel();
        _model.QueueDeclare(
            _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);
        
        // _model.ExchangeDeclare(
        //     configuration.GetSection("MyConfiguration:ExchangeName").Get<string>(), 
        //     ExchangeType.Fanout, 
        //     durable: true, autoDelete: 
        //     false);
        
        // _model.QueueBind(
        //     _queueName, 
        //     configuration.GetSection("MyConfiguration:ExchangeName").Get<string>(), 
        //     string.Empty);
    }
    
    public async Task ReadMessage()
    {
        var consumer = new AsyncEventingBasicConsumer(_model);
        consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var text = Encoding.UTF8.GetString(body);

            await _notificationService.SendNotification(new ReceivedNotification
            {
                Text = text
            });

            await Task.CompletedTask;
            _model.BasicAck(ea.DeliveryTag, false);
        };

        _model.BasicConsume(_queueName, false, consumer);
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