namespace NNotificator.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IEvent;
    Task<TResult> PublishAsync<T, TResult>(T message, CancellationToken cancellationToken = default) where T : IEvent;
}