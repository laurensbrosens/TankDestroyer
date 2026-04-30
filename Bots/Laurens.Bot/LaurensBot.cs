using TankDestroyer.API;

namespace HDJO.Bot;

[Bot("Laurens bot", "Laurens", "FFFFFF")]
public class LaurensBot : IPlayerBot
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