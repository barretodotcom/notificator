using NNotificator.Abstractions;

namespace NNotificator.Core;

public interface IEventHandler<in T, TResult> where T : IEvent
{
    Task<TResult> HandleAsync(T message, CancellationToken token = default);
}

public interface IEventHandler<in T> where T : IEvent
{
    Task HandleAsync(T message, CancellationToken token = default);
}