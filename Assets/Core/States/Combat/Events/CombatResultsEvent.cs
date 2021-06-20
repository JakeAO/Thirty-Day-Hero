using System;
using Core.CombatSettings;
using SadPumpkin.Util.CombatEngine.Events;

namespace Core.States.Combat.Events
{
    public class CombatResultsEvent : ICombatEventData
    {
        public readonly CombatResults Results;
        public readonly Action ResultsConfirmed;

        public CombatResultsEvent(CombatResults results, Action resultsConfirmed)
        {
            Results = results;
            ResultsConfirmed = resultsConfirmed;
        }
    }
}