using System.Collections.Generic;
using Core.EventOptions;
using Core.States.BaseClasses;
using SadPumpkin.Util.StateMachine;

namespace Core.States.Town
{
    public class TownInnState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";

        public override void OnContent()
        {
            _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>()
            {
                new EventOption(
                    "Leave",
                    GoToTownHub)
            };
        }

        private void GoToTownHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<TownHubState>();
        }
    }
}