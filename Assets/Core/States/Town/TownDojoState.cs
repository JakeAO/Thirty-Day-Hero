using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Town
{
    public class TownDojoState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Leave",
                GoToTownHub);
        }

        private void GoToTownHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownHubState>();
        }
    }
}