using System.Collections.Generic;
using Core.EventOptions;
using Core.States.BaseClasses;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Combat
{
    public class CombatEndState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";

        public override void OnContent()
        {
            _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>(3)
            {
                new EventOption(
                    "Continue",
                    GoToGameHub),
                new EventOption(
                    "Continue (All Dead)",
                    GoToDefeat),
                new EventOption(
                    "Continue (Calamity Vanquished)",
                    GoToVictory)
            };
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