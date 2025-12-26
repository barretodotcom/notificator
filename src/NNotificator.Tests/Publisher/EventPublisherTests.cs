using Microsoft.Extensions.DependencyInjection;
using NNotificator.Abstractions;
using NNotificator.DependencyInjection;
using NNotificator.Tests.Types.Events;
using NNotificator.Tests.Types.Handlers;
using Xunit;

namespace NNotificator.Tests.Publisher;

public class EventPublisherTests
{
    [Fact]
    public async Task Publish_Async_Should_Invoke_EventHandler()
    {
        var services = new ServiceCollection();
        
        services.AddNotificator(
            typeof(EventTestHandler).Assembly
            );

        var provider = services.BuildServiceProvider();

        var publisher = provider.GetRequiredService<IEventPublisher>();

        var @event = new TestEvent();

        await publisher.PublishAsync(@event);
        
        Assert.True(@event.WasRaised);
    }
}