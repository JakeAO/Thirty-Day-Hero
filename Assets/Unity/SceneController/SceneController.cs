using Core.States;
using SadPumpkin.Util.StateMachine.Signals;
using SadPumpkin.Util.StateMachine.States;
using System;

namespace Unity.Scenes
{
    public class SceneController : IDisposable
    {
        private readonly StateChanged _stateChangedSignal = null;

        public SceneController(StateChanged stateChangedSignal)
        {
            _stateChangedSignal = stateChangedSignal;
            _stateChangedSignal.Listen(OnStateChanged);
        }

        public void Dispose()
        {
            if(_stateChangedSignal != null)
            {
                _stateChangedSignal.Unlisten(OnStateChanged);
            }
        }

        private void OnStateChanged(IState newState)
        {
            switch (newState)
            {
                case CreatePartyState createParty:
                    {
                        UnityEngine.Debug.Log("LOAD PARTY CREATION SCENE");
                    }
                    break;
                case GameHubState gameHub:
                    {
                        UnityEngine.Debug.Log("GAME HUB SCENE");
                    }
                    break;
            }
        }
    }
}