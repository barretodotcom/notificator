using NNotificator.Abstractions;
using NNotificator.Core;
using NNotificator.Tests.Types.Events;

namespace NNotificator.Tests.Types.Handlers;

public class PublishResultTest
{
    public string TestResult { get; set; }

}

public class EventWithResultTestHandler : IEventHandler<TestEvent, PublishResultTest>
{
    public async Task<PublishResultTest> HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
    {
        return new PublishResultTest { TestResult = "Event handled" };
    }

}