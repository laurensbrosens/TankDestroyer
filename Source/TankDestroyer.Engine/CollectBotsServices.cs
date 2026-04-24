using System.Reflection;
using System.Runtime.Loader;
using TankDestroyer.API;

namespace TankDestroyer.Engine;

public static class CollectBotsServices
{
    static CollectBotsServices()
    {
      
        AssemblyLoadContext.GetLoadContext(typeof(CollectBotsServices).Assembly).Resolving += ResolveFindDll;
    }

    private static Assembly? ResolveFindDll(AssemblyLoadContext arg1, AssemblyName arg2)
    {
        Console.WriteLine($"Attempting to load {arg2.FullName}");
        var assembly = typeof(IPlayerBot).Assembly;
        return assembly;
    }


    public static Type[] LoadBots(string folder)
    {
        Console.WriteLine($"Loading bots from: {folder}");
        List<Type> allBots = new();
        var containingAssembly = typeof(IPlayerBot).Assembly;
        var typeOfPlayerBot = typeof(IPlayerBot);
        foreach (var dllFile in Directory.GetFiles(folder, "*.Bot.dll"))
        {
            Console.WriteLine($"Load from dll: {dllFile}");
            try
            {
                var assembly = AssemblyLoadContext.GetLoadContext(typeof(CollectBotsServices).Assembly).LoadFromAssemblyPath(dllFile);
                var botsInAssembly = assembly.ExportedTypes.Where(c =>
                    c.IsAssignableTo(typeof(IPlayerBot)) && c.GetCustomAttribute<BotAttribute>() != null);
                allBots.AddRange(botsInAssembly);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return allBots.ToArray();
    }
}