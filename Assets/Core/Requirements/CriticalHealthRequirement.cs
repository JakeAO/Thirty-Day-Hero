using Core.Etc;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;

using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Requirements
{
    public class CriticalHealthRequirement : IRequirementCalc
    {
        public static readonly CriticalHealthRequirement Instance = new CriticalHealthRequirement();
        
        public bool MeetsRequirement(IInitiativeActor actor)
        {
            if (actor is ICharacterActor characterActor)
            {
                return characterActor.Stats.GetStat(StatType.HP) <= characterActor.Stats.GetStat(StatType.HP_Max) * 0.2f;
            }

            return false;
        }
    }
}