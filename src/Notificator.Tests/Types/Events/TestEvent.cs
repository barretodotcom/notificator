using Notificator.Abstractions;

namespace Notificator.Tests.Types.Events;

public sealed class TestEvent : IEvent
{
    public bool WasRaised { get; set; }
}