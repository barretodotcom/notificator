using System;
using System.Threading;
using System.Threading.Tasks;
using Notificator.Abstractions;
using Notificator.Tests.Types.Events;

namespace Notificator.Tests.Types.Handlers;

public class EventTestHandler : IEventHandler<TestEvent>
{
    public async Task HandleAsync(TestEvent notification, CancellationToken cancellationToken)
    {
        notification.WasRaised = true;
        Console.WriteLine("Event was handled");
        await Task.CompletedTask;
    }
}