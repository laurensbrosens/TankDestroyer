namespace TankDestroyer.API;

public class BotAttribute : Attribute
{
    public string Name { get; }
    public string Creator { get; }
    public string Color { get; }

    public BotAttribute(string name, string creator, string color)
    {
        this.Name = name;
        Creator = creator;
        Color = color ?? "#000000";
    }
}