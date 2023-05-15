using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Loghmani.Localization;

public static class ServiceCollectionExtension
{
    public static void AddJsonLocalization(this IServiceCollection services, Action<JsonLocalizationOption> options)
    {
        services.Configure(options);

        services.AddLocalization();

        services.AddDistributedMemoryCache();

        services.AddSingleton<IStringLocalizerFactory, JsonLocalizerFactory>();

    }
}