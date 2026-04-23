namespace TankDestroyer.Engine;

public abstract class TankAction
{
    public int OwnerId { get; }

    internal int Priority { get; set; } = 0;

    public TankAction(int ownerId)
    {
        OwnerId = ownerId;
    }

    internal abstract bool Execute(Game game);
}