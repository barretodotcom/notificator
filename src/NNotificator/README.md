## Installation
```bash
dotnet add package NNotificator
```

## What is it?
NNotificator is a simple in-process event dispatcher designed for Domain-Driven Design (DDD),
allowing aggregates to raise domain events that are handled after persistence.

⸻
## Usage
Event
```bash
public sealed class ProductCreatedEvent : IEvent
{
   public Guid ProductId { get; }
   public ProductCreatedEvent(Guid productId)
   {
       ProductId = productId;
   }
}
```
Handler
```bash
public sealed class ProductCreatedHandler
   : IEventHandler<ProductCreatedEvent>
{
   public Task HandleAsync(
       ProductCreatedEvent @event,
       CancellationToken cancellationToken)
   {
       Console.WriteLine($"Product created: {@event.ProductId}");
       return Task.CompletedTask;
   }
}
```

DI
```bash
builder.Services.AddNotificator(
   typeof(ProductCreatedHandler).Assembly
);
```
Event publishing
```bash
await eventPublisher.PublishAsync(
   new ProductCreatedEvent(productId),
   CancellationToken.None
);
```
⸻
## Domain Events
Notificator is designed to work naturally with DDD aggregates.
Domain events can be collected inside aggregates and dispatched
after `SaveChanges` using an EF Core interceptor.
```bash

    public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
    {
        private readonly IEventPublisher _notificator;

        public DispatchDomainEventsInterceptor(IEventPublisher notificator)
        {
            _notificator = notificator;
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavedChanges(eventData, result);
        }

        public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;

            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(l => l.Entity.DomainEvents.Any())
                .Select(l => l.Entity);

            var domainEvents = aggregates.SelectMany(l => l.DomainEvents).ToList();

            aggregates.ToList().ForEach(l => l.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _notificator.PublishAsync(domainEvent);
        }
   }
```
⸻
## Limitations
- In-process only (no message broker)
- No retries or persistence
- No ordering guarantees

⸻

## License

MIT
