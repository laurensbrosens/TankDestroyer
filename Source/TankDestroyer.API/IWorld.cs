namespace TankDestroyer.API;

public interface IWorld
{
    public static bool AutoRun { get; set; } = true;
    ITile GetTile(int y, int x);
    int Height { get; set; }
    int Width { get; set; }
}