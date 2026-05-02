using TankDestroyer.API;

namespace HDJO.Bot;

[Bot("Laurens bot", "Laurens", "FFFFFF")]
public class LaurensBot : IPlayerBot
{
    private Direction _lastDirection = Direction.North;
    private ITurnContext _currentContext = null!;

    public void MoveWest()
    {
        _currentContext.MoveTank(Direction.East);
        _lastDirection = Direction.West;
    }

    public void MoveEast()
    {
        _currentContext.MoveTank(Direction.West);
        _lastDirection = Direction.East;
    }

    public void MoveNorth()
    {
        _currentContext.MoveTank(Direction.South);
        _lastDirection = Direction.North;
    }

    public void MoveSouth()
    {
        _currentContext.MoveTank(Direction.North);
        _lastDirection = Direction.South;
    }

    public TurretDirection Aim(ITank a, ITank b)
    {
        const float Threshold = 0.414f;
        float dx = b.X - a.X,
            dy = b.Y - a.Y;
        float absX = Math.Abs(dx),
            absY = Math.Abs(dy);
        TurretDirection d = 0;

        if (absY > absX * Threshold)
            d |= (b.Y > a.Y) ? TurretDirection.North : TurretDirection.South;
        if (absX > absY * Threshold)
            d |= (b.X > a.X) ? TurretDirection.West : TurretDirection.East;

        return d;
    }

    public void DoTurn(ITurnContext turnContext)
    {
        _currentContext = turnContext;
        var test = turnContext.GetMapHeight();
        var test2 = turnContext.GetMapWidth();

        var myTank = turnContext.Tank;
        var targetTank = turnContext.GetTanks().FirstOrDefault(t => t != myTank);

        turnContext.RotateTurret(Aim(myTank, targetTank));

        if (_lastDirection == Direction.North || _lastDirection == Direction.South)
        {
            if (targetTank.X > myTank.X)
            {
                MoveEast();
            }
            else if (targetTank.X < myTank.X)
            {
                MoveWest();
            }
            else
            {
                if (targetTank.Y > myTank.Y)
                {
                    MoveSouth();
                }
                else if (targetTank.Y < myTank.Y)
                {
                    MoveNorth();
                }
            }
        }
        else
        {
            if (targetTank.Y > myTank.Y)
            {
                MoveSouth();
            }
            else if (targetTank.Y < myTank.Y)
            {
                MoveNorth();
            }
            else
            {
                if (targetTank.X > myTank.X)
                {
                    MoveEast();
                }
                else if (targetTank.X < myTank.X)
                {
                    MoveWest();
                }
            }
        }

        turnContext.Fire();
    }
}
