using Microsoft.Extensions.Hosting;
using Notifications.Common.Interfaces;

namespace Notifications.BL.Services;

public class ReceiverHostedService: BackgroundService
{
    private readonly IReceiverService _receiverService;

    public ReceiverHostedService(IReceiverService receiverService)
    {
        _receiverService = receiverService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _receiverService.ReadMessage();
    }
}