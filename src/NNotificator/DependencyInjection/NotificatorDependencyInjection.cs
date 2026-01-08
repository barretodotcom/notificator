using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using NNotificator.Abstractions;
using NNotificator.Core;

namespace NNotificator.DependencyInjection;

public static class NNotificatorDependencyInjection
{
    public static void AddNotificator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IEventPublisher, EventPublisher>();

        var implementations = assemblies
            .SelectMany(l => l.GetTypes())
            .Where(l => !l.IsAbstract && !l.IsInterface)
            .SelectMany(t =>
                t.GetInterfaces()
                    .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IEventHandler<,>) || i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
                    .Select(l => new { Interface = l, Implementation = t })
            );

        foreach (var implementation in implementations)
        {
            services.AddScoped(implementation.Interface, implementation.Implementation);
        }

    }
}