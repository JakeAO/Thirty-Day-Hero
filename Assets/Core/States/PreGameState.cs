using System;
using System.Collections.Generic;
using Core.EventOptions;
using Core.Wrappers;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class PreGameState : TDHStateBase
    {
        public override void OnEnter(IState fromState)
        {
            if (SharedContext.Get<PlayerDataWrapper>() == null)
                throw new ArgumentException("Entered PreGameState without an active Player profile!");
            if (SharedContext.Get<PartyDataWrapper>() == null)
                throw new ArgumentException("Entered PreGameState without an active Party profile!");
        }

        public override IEnumerable<IEventOption> GetOptions()
        {
            yield return new EventOption(
                "Continue",
                GoToGameHub);
        }

        private void GoToGameHub()
        {
            SharedContext.Get<IStateMachine>().ChangeState<GameHubState>();
        }
    }
}