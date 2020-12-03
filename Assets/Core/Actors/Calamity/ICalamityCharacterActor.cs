using Core.Actors.Enemy;
using Core.Classes.Calamity;

namespace Core.Actors.Calamity
{
    public interface ICalamityCharacterActor : IEnemyCharacterActor
    {
        new ICalamityClass Class { get; }
    }
}