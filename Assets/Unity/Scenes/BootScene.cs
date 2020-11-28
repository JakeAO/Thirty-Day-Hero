using System;
using System.IO;
using Core.Etc;
using Core.States;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.Signals;
using UnityEngine;

namespace Unity.Scenes
{
    public class BootScene : SceneRootBase
    {
        private StartupState _state = null;

        private void Start()
        {
            // Create context
            Context = new Context();

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
            Context.Set(pathUtility);

            StateChanged stateChangedSignal = new StateChanged();
            Context.Set(stateChangedSignal);
            stateChangedSignal.Listen(newState => Debug.Log($"[StateMachine] Changed state to {newState.GetType()}"));

            // Create and add state machine
            IStateMachine stateMachine = new StateMachine(Context, stateChangedSignal);
            Context.Set(stateMachine);

            Context.Set(new SceneController(Context));

            // Initialize StartupState
            _state = new StartupState();
            stateMachine.ChangeState(_state);
        }

        protected override void OnInject()
        {
            throw new InvalidOperationException($"{nameof(BootScene)} cannot be injected into, it is supposed to be the very first state!");
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label("Debug flow:");
                if (_state == null)
                {
                    GUILayout.Label("Loading...");
                }
                else
                {
                    if (GUILayout.Button("Continue"))
                    {
                        _state.Continue();
                    }
                }
            }
            GUILayout.EndVertical();
        }
    }
}