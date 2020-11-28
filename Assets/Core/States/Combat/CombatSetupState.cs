using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States.Combat
{
    public class CombatSetupState : IState
    {
        public void PerformSetup(IContext context, IState previousState)
        {
            
        }

        public void PerformContent(IContext context)
        {
            context.Get<IStateMachine>().ChangeState<CombatMainState>();
        }

        public void PerformTeardown(IContext context, IState nextState)
        {
            
        }
    }
}