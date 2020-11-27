using Core.Classes.Enemy;

namespace Core.Actors.Enemy
{
    public interface IEnemyCharacterActor : ICharacterActor
    {
        new IEnemyClass Class { get; }
    }
}