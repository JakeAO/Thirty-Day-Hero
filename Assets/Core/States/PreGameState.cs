using System;
using Core.Wrappers;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class PreGameState : IState
    {
        public void PerformSetup(IContext context, IState previousState)
        {
            if (context.Get<PlayerDataWrapper>() == null)
                throw new ArgumentException("Entered PreGameState without an active Player profile!");
            if (context.Get<PartyDataWrapper>() == null)
                throw new ArgumentException("Entered PreGameState without an active Party profile!");
        }

        public void PerformContent(IContext context)
        {
            
        }

        public void PerformTeardown(IContext context, IState nextState)
        {
            
        }
    }
}