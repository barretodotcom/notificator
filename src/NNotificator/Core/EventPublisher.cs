using Microsoft.Extensions.DependencyInjection;
using NNotificator.Abstractions;

namespace NNotificator.Core;

public class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var eventType = message.GetType();

        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

        var handlers = _serviceProvider.GetServices(handlerType);

        var tasks = handlers.Cast<dynamic>().Select(l => (Task)l.HandleAsync((dynamic)message, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public async Task<TResult> PublishAsync<T, TResult>(T message, CancellationToken cancellationToken = default)
        where T : IEvent
    {
        var eventType = message.GetType();

        var handlerType = typeof(IEventHandler<,>).MakeGenericType(eventType, typeof(TResult));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null) throw new InvalidOperationException($"No handler registered for {eventType}");

        TResult result = await ((dynamic)handler).HandleAsync((dynamic)message, cancellationToken);

        return result;
    }
}