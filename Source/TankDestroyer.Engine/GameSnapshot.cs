namespace TankDestroyer.Engine;

public class GameSnapshot
{
    public int Turn { get; set; } = 0;
    public World World { get; set; }
    public Tank[] Tanks { get; set; }
    public PlayerBot Players { get; set; }
}