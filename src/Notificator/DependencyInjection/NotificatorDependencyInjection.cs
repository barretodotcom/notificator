using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Notificator.Abstractions;
using Notificator.Core;

namespace Notificator.DependencyInjection;

public static class NotificatorDependencyInjection
{
    public static void AddNotificator(this IServiceCollection services)
    {
        services.AddScoped<IEventPublisher, EventPublisher>();

        var handlerTypes = AppDomain.CurrentDomain.GetAssemblies();

        var implementations = handlerTypes
            .SelectMany(l => l.GetTypes())
            .Where(l => !l.IsAbstract & !l.IsInterface)
            .SelectMany(t =>
                t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    .Select(l => new { Interface = l, Implementation = t })
            );

        foreach (var implementation in implementations)
        {
            services.AddScoped(implementation.Interface, implementation.Implementation);
        }

    }
}