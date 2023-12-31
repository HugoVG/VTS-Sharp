using Microsoft.Extensions.DependencyInjection;

namespace VTS.Core.Examples.Advanced.Examples;

public static class ExampleLoaders
{
    public static IServiceCollection AddExamples(this IServiceCollection services)
    {
        //Get all types that implement IExample
        var exampleTypes = typeof(ExampleLoaders).Assembly.GetTypes().Where(t => typeof(IExample).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (var exampleType in exampleTypes)
        {
            //Add each example to the service collection
            services.AddScoped(exampleType);
        }
        return services;
    }
}