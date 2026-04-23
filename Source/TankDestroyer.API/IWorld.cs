namespace TankDestroyer.API;

public interface IWorld
{
    ITile GetTile(int y, int x);
}