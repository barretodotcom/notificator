using NNotificator.Abstractions;

namespace NNotificator.Core;

public abstract class EventHandler<T> : IEventHandler<T> where T : IEvent
{
    public async Task HandleAsync(T message, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }
}