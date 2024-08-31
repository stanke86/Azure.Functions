using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QueueExample.Services;

public static class ServiceCollectionExtension
{
    public static void AddQueueExampleServices(this IServiceCollection sc, IConfiguration configuration)
    {
        // Options pattern: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0#the-options-pattern
        // If you need to register it with DI, use DI services to configure options: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0#use-di-services-to-configure-options

        // Requires Microsoft.Extensions.Options.ConfigurationExtensions NuGet
        sc.Configure<ServiceSettings>(configuration.GetSection(ServiceSettings.SectionName));

        sc.AddScoped<IQueueService, QueueService>();
    }
}