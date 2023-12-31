using VTS.Core.Examples.Advanced.CustomImplementations.Services;
using VTS.Core.Examples.Advanced.Models;

namespace VTS.Core.Examples.Advanced;

public class Plugin(IServiceProvider services, IVTSLogger logger, PluginInfo pluginInfo)
{
    public CoreVTSPlugin VTSPlugin { get; private set; }
    public async void Start()
    {
        WebSocketImpl websocket = new(logger);
        NewtonsoftJsonUtilityImpl jsonUtility = new();
        TokenStorageImpl tokenStorage = new("");
        logger.Log("Connecting...");
        logger.Log(pluginInfo.Value.PluginName);
        VTSPlugin = new(logger, pluginInfo.Value.UpdateInterval, pluginInfo.Value.PluginName, pluginInfo.Value.PluginAuthor, pluginInfo.Value.PluginIcon);
        logger.Log($"Plugin Version: {pluginInfo.Value.PluginVersion}");
        try {
            await VTSPlugin.InitializeAsync(websocket, jsonUtility, tokenStorage, () => logger.LogWarning("Disconnected!")); 
            logger.Log("Connected!");
        } catch (VTSException e) {
            logger.LogError(e); // VTS probably isn't running
        }
        SubscribeToEvents(VTSPlugin, logger);
        
        await LogVtsInfo(VTSPlugin, logger);
        
        // DI Setup
        
    }
    private static async Task LogVtsInfo(CoreVTSPlugin plugin, IVTSLogger logger)
    {
        var apiState = await plugin.GetAPIState();
        logger.Log($"Using VTubeStudio {apiState.data.vTubeStudioVersion}");

        var currentModel = await plugin.GetCurrentModel();
        logger.Log($"The current model is: {currentModel.data.modelName}");
    }
    
    private void SubscribeToEvents(CoreVTSPlugin plugin, IVTSLogger logger)
    {
        plugin.SubscribeToBackgroundChangedEvent((backgroundInfo) =>
        {
            logger.Log($"The background was changed to: {backgroundInfo.data.backgroundName}");
        });
        plugin.SubscribeToItemEvent(new VTSItemEventConfigOptions(), data =>
        {
            logger.Log($"Item event: {data.data.itemInsanceID}");
        });
    }
}