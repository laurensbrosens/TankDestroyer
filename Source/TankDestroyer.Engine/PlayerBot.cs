using TankDestroyer.API;

namespace TankDestroyer.Engine;

public class PlayerBot
{
    public PlayerBot(IPlayerBot playerImplementation, int id)
    {
        PlayerImplementation = playerImplementation;
        Id = id;
    }

    public int Id { get; set; }
    public IPlayerBot PlayerImplementation { get; set; }
    
}
