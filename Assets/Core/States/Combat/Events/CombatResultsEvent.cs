using Core.CombatSettings;
using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat.Events
{
    public class CombatResultsEvent : ICombatEventData
    {
        public readonly CombatResults Results;

        public CombatResultsEvent(CombatResults results)
        {
            Results = results;
        }
    }
}