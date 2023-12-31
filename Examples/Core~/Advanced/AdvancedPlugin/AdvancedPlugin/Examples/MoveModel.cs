using VTS.Core.Examples.Advanced.CustomImplementations.Services;

namespace VTS.Core.Examples.Advanced.Examples;

/// <summary>
/// Example how to move the model to a specific position
/// </summary>
public class MoveModel(IVTSLogger logger, Plugin plugin) : IExample
{
    public async Task Perform()
    {
        var curr = await plugin.VTSPlugin.GetCurrentModel(); // Get the current model for the position and size
        Tuple<float,float> coordsToMoveTo = new(0,0);
        if(curr.data.modelPosition.positionX == 0 && curr.data.modelPosition.positionY == 0)
        {
            // this is so we can show the model moving in the example even if it is already at 0,0
            coordsToMoveTo = new(-0.8f,-0.8f); 
        }
        logger.Log($"Moving model to {coordsToMoveTo.Item1}, {coordsToMoveTo.Item2}");
        
        // performs the move
        await plugin.VTSPlugin.MoveModel(new()
        {
            positionX = coordsToMoveTo.Item1,
            positionY = coordsToMoveTo.Item2,
            size = curr.data.modelPosition.size,
            timeInSeconds = 1
        });
    }
}