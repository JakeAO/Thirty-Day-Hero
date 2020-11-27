using Core.Classes;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Actors
{
    public interface ICharacterActor : ITargetableActor
    {
        IClass Class { get; }
        IStatMap Stats { get; }
        
        float GetReducedDamage(float damageAmount, DamageType damageType);
    }
}