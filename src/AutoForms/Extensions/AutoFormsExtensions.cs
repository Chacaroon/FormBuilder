using AutoForms.Resolvers;
using AutoForms.Strategies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoForms.Extensions;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>
/// </summary>
public static class AutoFormsExtensions
{
    /// <summary>
    /// Register services required by AutoForms.
    /// </summary>
    /// <param name="serviceCollection">.</param>
    /// <returns>The same instance of the <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAutoForms(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(services =>
            new StrategyResolver(
                services.GetRequiredService<IServiceProvider>()));

        serviceCollection.AddScoped(services =>
            new FormBuilderFactory(services.GetRequiredService<StrategyResolver>()));

        var strategies = Assembly.GetAssembly(typeof(BaseStrategy))!
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => typeof(BaseStrategy).IsAssignableFrom(x));

        foreach (var strategyType in strategies)
        {
            serviceCollection.AddTransient(typeof(BaseStrategy), strategyType);
        }

        return serviceCollection;
    }

    /// <summary>
    /// Register Newtonsoft.Json serializer with predefined settings.
    /// </summary>
    /// <param name="mvcBuilder"></param>
    /// <returns><see cref="IMvcBuilder"/></returns>
    [ExcludeFromCodeCoverage]
    public static IMvcBuilder AddAutoFormsSerializer(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

        return mvcBuilder;
    }

    /// <summary>
    /// Register Newtonsoft.Json serializer with predefined settings.
    /// </summary>
    /// <param name="mvcBuilder"></param>
    /// <param name="action">Callback to configure <see cref="MvcNewtonsoftJsonOptions"/>.</param>
    /// <returns><see cref="IMvcBuilder"/></returns>
    [ExcludeFromCodeCoverage]
    public static IMvcBuilder AddAutoFormsSerializer(this IMvcBuilder mvcBuilder, Action<MvcNewtonsoftJsonOptions> action)
    {
        mvcBuilder.AddNewtonsoftJson(options =>
        {
            action(options);
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

        return mvcBuilder;
    }
}
