using Microsoft.AspNetCore.Mvc;
using Notifications.Common.Dto;
using Notifications.Common.Interfaces;

namespace Notifications.Controllers;

public class NotificationController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task SendNotification(ReceivedNotification notification)
    {
        await _notificationService.SendNotification(notification);
    }
}