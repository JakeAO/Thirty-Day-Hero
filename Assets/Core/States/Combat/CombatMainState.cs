using System;
using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatMainState : TDHStateBase
    {
        public override void OnEnter(IState fromState)
        {
            if (SharedContext.Get<CombatSettings.CombatSettings>() == null)
                throw new ArgumentException("Entered CombatMain without an active CombatSettings!");
        }

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