
using HitMarker.Utils;

namespace HitMarker.Console.Commands.Give
{
    public class GiveCommand : ICommand
    {
        public string Name => "give";
        public string Description => "Spawns a craftable item by its index. Usage: givecraftable <index>";


        public void Execute(string args)
        {
            if (args == null || args.Equals(""))
            {
                LoggerUtils.LogDebug("GiveCraftableCommand", "Usage: givecraftable <index>");
                return;
            }

            ModSpawnHelper.SpawnItem(args);
            LoggerUtils.LogDebug("GiveCraftableCommand", $"Item with index {args[0]} spawned successfully.");
        }
    }
}
