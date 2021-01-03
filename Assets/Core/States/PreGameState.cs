using System;
using System.Collections.Generic;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class PreGameState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";

        public override void OnEnter(IState fromState)
        {
            if (SharedContext.Get<PlayerDataWrapper>() == null)
                throw new ArgumentException("Entered PreGameState without an active Player profile!");
            if (SharedContext.Get<PartyDataWrapper>() == null)
                throw new ArgumentException("Entered PreGameState without an active Party profile!");
        }

        public override void OnContent()
        {
            _currentOptions[CATEGORY_DEFAULT] = new List<IEventOption>()
            {
                new EventOption(
                    "Continue",
                    GoToGameHub)
            };
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}