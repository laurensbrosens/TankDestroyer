namespace TankDestroyer.API;

public interface ITank
{
    int X { get; }
    int Y { get; }
    int Health { get; }
    TurretDirection TurretDirection { get; }
    bool Destroyed { get; }
    bool Fired { get; }
    int OwnerId { get;  }
}

[Flags]
public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 4,
}

[Flags]
public enum TurretDirection
{
    North = 1,
    East = 2,
    South = 4,
    West = 8,
    NorthEast = North | East,
    NorthWest = North | West,
    SouthEast = South | East,
    SouthWest = South | West
}