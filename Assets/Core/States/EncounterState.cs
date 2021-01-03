using System.Collections.Generic;
using Core.EventOptions;
using Core.States.BaseClasses;
using SadPumpkin.Util.StateMachine;

namespace Core.States
{
    public class EncounterState : TDHStateBase
    {
        public const string CATEGORY_CONTINUE = "Continue";

        public override void OnContent()
        {
            _currentOptions[CATEGORY_CONTINUE] = new List<IEventOption>()
            {
                new EventOption(
                    "Leave Encounter",
                    GoToGameHub, CATEGORY_CONTINUE)
            };
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}