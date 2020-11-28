using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Combat
{
    public class CombatSetupState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Begin Fight",
                GoToCombatMain);
        }

        private void GoToCombatMain()
        {
            SharedContext.Get<IStateMachine>().ChangeState<CombatMainState>();
        }
    }
}