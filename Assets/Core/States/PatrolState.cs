using System.Collections.Generic;
using Core.EventOptions;
using Core.States.Combat;
using SadPumpkin.Util.StateMachine;

namespace Core.States
{
    public class PatrolState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Fight",
                GoToCombat,
                priority: 0);
            yield return new EventOption(
                "Sneak Away",
                GoToGameHub,
                priority: 1);
        }

        private void GoToCombat()
        {
            SharedContext.Get<IStateMachine>().ChangeState<CombatSetupState>();
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}