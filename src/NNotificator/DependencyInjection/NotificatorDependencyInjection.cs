using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using NNotificator.Abstractions;
using NNotificator.Core;

namespace NNotificator.DependencyInjection;

public static class NNotificatorDependencyInjection
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