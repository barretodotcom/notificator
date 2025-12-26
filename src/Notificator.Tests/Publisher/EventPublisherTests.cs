using Microsoft.Extensions.DependencyInjection;
using Notificator.Abstractions;
using Notificator.DependencyInjection;
using Notificator.Tests.Types.Events;
using Xunit;

namespace Notificator.Tests.Publisher;

public class EventPublisherTests
{
    [Fact]
    public async Task Publish_Async_Should_Invoke_EventHandler()
    {
        var services = new ServiceCollection();
        
        services.AddNotificator();

        var provider = services.BuildServiceProvider();

        var publisher = provider.GetRequiredService<IEventPublisher>();

        var @event = new TestEvent();

        await publisher.PublishAsync(@event);
        
        Assert.True(@event.WasRaised);

    }
}