namespace VTS.Core.Examples.Advanced.Examples;

public class PinObjectToModel(IVTSLogger logger, Plugin plugin) : IExample
{
    public async Task Perform()
    {
        logger.Log($"Pinning object to model");
        
        //Get all items
        var items = await plugin.VTSPlugin.GetItemList(new ()
        {
            includeAvailableSpots = true,
            includeAvailableItemFiles = true,
        });

        // Check if plugin has permission to load items
        
        if (items.data.canLoadItemsRightNow == false)
        {
            logger.Log("Unable to load items right now, user is probably in a menu");
            return;
        }
        
        // Get the first item that is a prop
        var prop = items.data.availableItemFiles.FirstOrDefault();
        if (prop == null)
        {
            logger.Log("No props available");
            return;
        }
        
        // Get the first available spot
        var spot = items.data.availableSpots.FirstOrDefault();
        if (spot == default)
        {
            logger.Log("No spots available");
            return;
        }
        
        // Load the item
        var item = await plugin.VTSPlugin.LoadItem(prop.fileName, new VTSCustomDataItemLoadOptions()
        {
            positionX = 0,
            positionY = 0,
            size = 0.01f
        });
        
        //get Item info
        var itemInstanceId = item.data.instanceID;
        // Get Current Model Info
        var model = await plugin.VTSPlugin.GetCurrentModel();
        
        // Get a list of artmeshes
        var artMeshes = await plugin.VTSPlugin.GetArtMeshList();
        
        
        if(artMeshes?.data?.artMeshNames == null || artMeshes.data.artMeshNames.Length == 0)
        {
            logger.Log("No artmeshes available");
            return;
        }
        
        // Get the first artmesh
        Random random = new();
        
        var artMesh = artMeshes.data.artMeshNames[random.Next(0, artMeshes.data.artMeshNames.Length)];

        // Model Id and ArtMesh Id left empty for random
        await plugin.VTSPlugin.PinItemToRandom(itemInstanceId, model.data.modelID, artMesh, 0,
            VTSItemAngleRelativityMode.RelativeToModel, 0.01f, VTSItemSizeRelativityMode.RelativeToWorld);

    }
}