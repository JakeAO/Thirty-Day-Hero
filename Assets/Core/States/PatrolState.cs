using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class PatrolState : IState
    {
        public void PerformSetup(IContext context, IState previousState)
        {
            
        }

        public void PerformContent(IContext context)
        {
            //auto-default to combat
            //maybe we built in a way to identify/try to escape?
            context.Get<IStateMachine>().ChangeState<Combat.CombatSetupState>();
        }

        public void PerformTeardown(IContext context, IState nextState)
        {
            
        }
    }
}