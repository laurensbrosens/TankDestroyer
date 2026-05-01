using System.Reflection;
using TankDestroyer.API;

namespace TankDestroyer.Engine;

public static class CollectBotsServices
{
    public static Type[] LoadBots(string buildFolderPath)
    {
        var allBots = new List<Type>();
        var typeOfPlayerBot = typeof(IPlayerBot);

        // Get all DLLs in your custom Build/Bots folder
        var dllFiles = Directory.GetFiles(buildFolderPath, "*.dll");

        foreach (var file in dllFiles)
        {
            try
            {
                // Assembly.LoadFrom handles dependencies better than LoadFromAssemblyPath
                // for shared locations like your Build folder.
                var assembly = Assembly.LoadFrom(file);

                var bots = assembly.GetExportedTypes().Where(t =>
                    typeOfPlayerBot.IsAssignableFrom(t) &&
                    !t.IsInterface &&
                    !t.IsAbstract &&
                    t.GetCustomAttribute<BotAttribute>() != null);

                allBots.AddRange(bots);
            }
            catch (BadImageFormatException) { /* Skip non-managed DLLs */ }
            catch (Exception ex) { Console.WriteLine($"Failed to load {file}: {ex.Message}"); }
        }

        return allBots.ToArray();
    }
}