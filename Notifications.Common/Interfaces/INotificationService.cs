using Notifications.Common.Dto;

namespace Notifications.Common.Interfaces;

public interface INotificationService
{
    Task SendNotification(ReceivedNotification notification);
}