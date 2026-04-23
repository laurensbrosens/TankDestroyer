using TankDestroyer.API;

namespace TankDestroyer.Engine;

public class Tile : ITile
{
    public TileType TileType { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    
}