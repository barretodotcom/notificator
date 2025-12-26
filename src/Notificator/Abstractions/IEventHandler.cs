namespace Notificator.Abstractions;

public interface IEventHandler<in T> where T : IEvent
{
    Task HandleAsync(T message, CancellationToken token = default);
}