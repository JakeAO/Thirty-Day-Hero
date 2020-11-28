using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.StateMachine;

namespace Core.States
{
    public class EncounterState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Leave Encounter",
                GoToGameHub);
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}