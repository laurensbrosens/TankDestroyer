namespace TankDestroyer.API;

public interface ITile
{
    TileType TileType { get; set; }
    int X { get; set; }
    int Y { get; set; }
}