using System;
using Core.Wrappers;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class CreatePartyState : IState
    {
        public void PerformSetup(IContext context, IState previousState)
        {
            if (context.Get<PlayerDataWrapper>() == null)
                throw new ArgumentException("Entered CreatePartyState without an active Player profile!");
            if (context.Get<PartyDataWrapper>() != null)
                throw new ArgumentException("Entered CreatePartyState with an already active Party profile!");
        }

        public void PerformContent(IContext context)
        {
            int nine = 9;
            nine += 7;
            Console.WriteLine(nine);
        }

        public void PerformTeardown(IContext context, IState nextState)
        {
            
        }
    }
}