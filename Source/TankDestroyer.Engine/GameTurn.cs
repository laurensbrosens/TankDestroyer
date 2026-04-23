namespace TankDestroyer.Engine;

public class GameTurn
{
    public int Turn { get; set; } = 0;
    public World World { get; set; }
    public Tank[] Tanks { get; set; }

    public TankAction[] Actions { get; set; }
    public Bullet[] Bullets { get; set; }
}