using System.Collections.Generic;
using Core.Etc;
using Core.EventOptions;
using Core.States.Combat;
using Core.States.Town;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class GameHubState : TDHStateBase
    {
        public PartyDataWrapper PartyData { get; private set; }

        public override void OnContent()
        {
            PartyData = SharedContext.Get<PartyDataWrapper>();
        }

        public override void OnExit(IState toState)
        {
            PartyData.IncrementTime();
            SaveLoadHelper.SavePartyData(SharedContext);
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            if (PartyData.Day >= Constants.DAYS_TO_PREPARE)
            {
                yield return new EventOption(
                    "Face the Calamity",
                    GoToCalamity);
                yield break;
            }

            yield return new EventOption(
                "Rest at Camp",
                GoToRest,
                priority: 0);
            yield return new EventOption(
                "Enter Town",
                GoToTownHub,
                priority: 1);
            yield return new EventOption(
                "Patrol Area",
                GoToPatrol,
                priority: 2);
            yield return new EventOption(
                "Search Area (Encounter)",
                GoToEncounter,
                priority: 3);
        }

        private void GoToRest()
        {
            SharedContext.Get<IStateMachine>().ChangeState<RestState>();
        }
        
        private void GoToTownHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownHubState>();
        }

        private void GoToPatrol()
        {
            SharedContext.Get<IStateMachine>().ChangeState<PatrolState>();
        }

        private void GoToEncounter()
        {
            SharedContext.Get<IStateMachine>().ChangeState<EncounterState>();
        }

        private void GoToCalamity()
        {
            SharedContext.Get<IStateMachine>().ChangeState<CombatSetupState>();
        }
    }
}