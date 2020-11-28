using System.IO;
using Core.Etc;
using Core.States;
using Core.Wrappers;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.Signals;
using UnityEngine;

namespace Unity.Scenes
{
    public class BootScene : SceneRootBase<StartupState>
    {
        private void Start()
        {
            // Create context
            SharedContext = new Context();
            State = new StartupState();

            // Create and add PathUtility
            PathUtility pathUtility = new PathUtility(
                "localUser",
                Path.Combine(Application.persistentDataPath, "SaveData"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Ability"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "PlayerClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "EnemyClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "CalamityClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "EnemyGroup"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Item"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Weapon"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Armor"));
            SharedContext.Set(pathUtility);

            StateChanged stateChangedSignal = new StateChanged();
            SharedContext.Set(stateChangedSignal);
            stateChangedSignal.Listen(newState => Debug.Log($"[StateMachine] Changed state to {newState.GetType()}"));

            // Create and add state machine
            IStateMachine stateMachine = new StateMachine(SharedContext, stateChangedSignal);
            SharedContext.Set(stateMachine);

            // Initialize StartupState
            stateMachine.ChangeState(State);

            // Setup SceneController AFTER changing to first state, otherwise enter an infinite loop of reloading Boot
            SharedContext.Set(new SceneController(SharedContext));
        }

        protected override void OnGUIContentForState()
        {
            GUILayout.Label($"Player Loaded: {SharedContext.TryGet(out PlayerDataWrapper _)}");
            GUILayout.Label($"Party Loaded: {SharedContext.TryGet(out PartyDataWrapper _)}");
        }
    }
}