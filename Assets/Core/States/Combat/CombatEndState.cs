using System.Collections.Generic;
using Core.EventOptions;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Combat
{
    public class CombatEndState : TDHStateBase
    {
        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Continue",
                GoToGameHub);
            yield return new EventOption(
                "Continue (All Dead)",
                GoToDefeat);
            yield return new EventOption(
                "Continue (Calamity Vanquished)",
                GoToVictory);
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }

        private void GoToDefeat()
        {
            SharedContext.Get<IStateMachine>().ChangeState<DefeatState>();
        }

        private void GoToVictory()
        {
            SharedContext.Get<IStateMachine>().ChangeState<VictoryState>();
        }
    }
}