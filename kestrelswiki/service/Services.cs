using Microsoft.Extensions.DependencyInjection;

namespace kestrelswiki.service;

public static class Services
{
    public static IServiceProvider? Provider { get; private set; }

    public static Try<T> GetService<T>() where T : notnull
    {
        if(Provider is null) return new Exception("Provider is not initialized");

        using IServiceScope scope = Provider.CreateScope();
        T? service = scope.ServiceProvider.GetService<T>();
        if(service is null) return new Exception("Service is not configured");

        return service;
    }

    public static void Init(IServiceProvider provider)
    {
        if(Provider is not null) return;
        Provider = provider;
    }
}