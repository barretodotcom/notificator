using System;
using System.Threading;
using System.Threading.Tasks;
using NNotificator.Abstractions;
using NNotificator.Tests.Types.Events;

namespace NNotificator.Tests.Types.Handlers;

public class EventTestHandler : IEventHandler<TestEvent>
{
    public async Task HandleAsync(TestEvent notification, CancellationToken cancellationToken)
    {
        notification.WasRaised = true;
        Console.WriteLine("Event was handled");
        await Task.CompletedTask;
    }
}