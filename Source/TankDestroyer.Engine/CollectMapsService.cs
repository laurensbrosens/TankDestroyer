namespace TankDestroyer.Engine;

public class CollectMapsService
{
    public static World[] LoadMaps(string folder)
    {
        List<World> worlds = new();
        foreach (var filePath in Directory.EnumerateFiles(folder, "*.map"))
        {
            worlds.Add(World.LoadFromFile(filePath));
        }
        return worlds.ToArray();
    }
}