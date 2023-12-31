using VTS.Core.Examples.Advanced.CustomImplementations.Services;

namespace VTS.Core.Examples.Advanced.Examples;

/// <summary>
/// Example how to move the model to a specific position
/// </summary>
public class CustomParameters(IVTSLogger logger, Plugin plugin) : IExample
{
    // If you want to use this parameter in a script, you want to make sure that the name is unique
    // You might want to prefix it with your plugin name
    // Parameter names cannot contain spaces and have to be between 4 and 32 characters
    private const string YourCustomParameter = "YourCustomParameter";
    public async Task Perform()
    {
        logger.Log("Creating custom parameter");
        await plugin.VTSPlugin.AddCustomParameter(new VTSParameterCreationData.Data()
        {
            parameterName = YourCustomParameter,
            explanation = $"Cutom Parameter made by {plugin.VTSPlugin.PluginName} as an example", // max 256 characters
            defaultValue = 1,
            min = 0, // cannot be smaller than -1000000
            max = 10 // cannot be larger than 1000000
        });
    }
    
    // Not executed in the example, but you can use this to get the value of the parameter
    public async Task GetParameter()
    {
        logger.Log("Getting custom parameter");
        var parameter = await plugin.VTSPlugin.GetParameterValue(YourCustomParameter); 
        logger.Log($"Parameter value is {parameter.data.value}");
    }
    
    // Not executed in the example, but you can use this to set the value of the parameter
    public async Task SetParameter()
    {
        logger.Log("Setting custom parameter");
        await plugin.VTSPlugin.InjectParameterValues(new VTSParameterInjectionValue[]
        {
            new()
            {
                id = YourCustomParameter, 
                value = 5, // Value is the new value of the parameter
                weight = 1, // Weight is added to the parameter, so if you do value 1 and weight 0.5, the old value being 2 would become 1.5
            }
        }, VTSInjectParameterMode.SET); // SET will set the value of the parameter, ADD will add the value to the parameter
    }
    
    // Not executed in the example, but you can use this to remove the parameter
    public async Task RemoveParameter()
    {
        logger.Log("Removing custom parameter");
        await plugin.VTSPlugin.RemoveCustomParameter(YourCustomParameter); // This will remove the parameter from the list of parameters
    }
}