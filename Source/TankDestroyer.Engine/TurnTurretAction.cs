using TankDestroyer.API;

namespace TankDestroyer.Engine;

public class TurnTurretAction : TankAction
{
    private readonly TurretDirection _direction;

    public TurnTurretAction(int playerId, TurretDirection direction) : base(playerId)
    {
        _direction = direction;
        Priority = 0;
    }

    internal override bool Execute(Game game)
    {
        var tank = game.Tanks.FirstOrDefault(c => c.OwnerId == OwnerId);
        if (tank == null)
        {
            return false;
        }

        tank.TurretDirection = _direction;
        return true;
    }
}