using NNotificator.Abstractions;

namespace NNotificator.Tests.Types.Events;

public sealed class TestEvent : IEvent
{
    public bool WasRaised { get; set; }
}