namespace TankDestroyer.API;

public interface IBullet
{
    public int X { get; }
    public int Y { get; }
    TurretDirection Direction { get;  }
}