using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Combat
{
    public class CombatMainState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Win Combat",
                GoToCombatEnd);
            yield return new EventOption(
                "Lose Combat",
                GoToCombatEnd);
        }

        private void GoToCombatEnd()
        {
            SharedContext.Get<IStateMachine>().ChangeState<CombatEndState>();
        }
    }
}