using Core.CombatSettings;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatEndState : IState
    {
        private readonly CombatResults _result = null;

        public CombatEndState(CombatResults result)
        {
            _result = result;
        }

        public void PerformSetup(IContext context, IState previousState)
        {
            
        }

        public void PerformContent(IContext context)
        {
            if(_result.Success)
            {
                context.Get<IStateMachine>().ChangeState<GameHubState>();
            }
            else
            {
                context.Get<IStateMachine>().ChangeState<CreatePartyState>();
            }
        }

        public void PerformTeardown(IContext context, IState nextState)
        {
            
        }
    }
}