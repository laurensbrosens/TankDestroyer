using TankDestroyer.API;

namespace Example.Bot;

[Bot("Example bot", "Een goeie ontwikkelaar", "F527A6")]
public class ExampleBot : IPlayerBot
{
    private Random _random = new();

    public void DoTurn(ITurnContext turnContext)
    {
        var enumValues = Enum.GetValues<TurretDirection>();
        var enumDirectionValues = Enum.GetValues<Direction>();

        turnContext.MoveTank(enumDirectionValues[_random.Next(0, enumDirectionValues.Length)]);
        turnContext.RotateTurret(enumValues[_random.Next(0, enumValues.Length)]);

        turnContext.Fire();
    }
}