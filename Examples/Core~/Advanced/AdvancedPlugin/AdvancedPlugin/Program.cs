using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VTS.Core;
using VTS.Core.Examples.Advanced;
using VTS.Core.Examples.Advanced.CustomImplementations.Services;
using VTS.Core.Examples.Advanced.Examples;
using VTS.Core.Examples.Advanced.Models;

// Not yet AOT compatible

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args); // Create a new HostBuilder

// Configuration is optional, but recommended
builder.Configuration.AddJsonFile("appsettings.json", optional: true);


builder.Services.AddSingleton<IVTSLogger, ILoggerVTSLogger>(); // Add the ILogger implementation to show what is going on inside the plugin
builder.Services.AddSingleton<PluginInfo>(); // Add the PluginInfo implementation adds your plugin's information to the plugin list in VTube Studio
builder.Services.AddSingleton<Plugin>(); // Add the Plugin implementation to start your plugin
builder.Services.AddExamples(); // Add the examples to the service collection
var host = builder.Build(); // Build the host

var plugin = host.Services.GetRequiredService<Plugin>(); // Get the plugin from the service provider
plugin.Start(); // Start the plugin

while(!plugin.VTSPlugin.IsAuthenticated)
    await Task.Delay(100); // Wait for the plugin to authenticate

// Run Examples (Optional) - This will run all examples
var exampleTypes = typeof(ExampleLoaders).Assembly.GetTypes().Where(t => typeof(IExample).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
foreach (var exampleType in exampleTypes)
{
    if(host.Services.GetRequiredService(exampleType) is not IExample example)
        continue;
    await example.Perform();
}

await host.RunAsync(); // This will keep the program running until the user presses Ctrl+C, or kills the process


