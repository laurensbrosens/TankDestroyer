using System.Text;
using TankDestroyer.API;

namespace HDJO.Bot;

[Bot("HDJO bot", "Hendrik", "F54927")]
public class HDJOBot : IPlayerBot
{
    private Random _random = new();
    private bool _once;

    public void DoTurn(ITurnContext turnContext)
    {
      /*  if (turnContext.Tank.Y > 10)
        {
            // turnContext.MoveTank(Direction.);
        }
        else
        {*/
            var enumValues = Enum.GetValues<TurretDirection>();
            var enumDirectionValues = Enum.GetValues<Direction>();

            // turnContext.MoveTank(enumDirectionValues[_random.Next(0, enumDirectionValues.Length)]);
            //    turnContext.RotateTurret(enumValues[_random.Next(0, enumValues.Length)]);
            //    turnContext.RotateTurret(enumValues[_random.Next(0, enumValues.Length)]);
            /*  if (!_once)
              {*/
           // turnContext.MoveTank(Direction.North);
            turnContext.RotateTurret(TurretDirection.SouthEast);
            if (!_once)
            {
                turnContext.Fire();
                _once = true;
            }
            //} // turnContext.(Direction.North);
        //}
    }
}