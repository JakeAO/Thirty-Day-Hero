using System;
using System.IO;
using Core.Etc;
using Core.States;
using Core.States.BaseClasses;
using Core.Wrappers;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.Signals;
using Unity.Extensions;
using Unity.Interface;
using UnityEngine;

namespace Unity.Scenes
{
    public class BootScene : SceneRootBase<StartupState>
    {
        [SerializeField] private ButtonWithLabel _playButton;

        private void Start()
        {
            // Button Hookup
            _playButton.gameObject.UpdateActive(false);

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

            // Hackily call the Inject method, since we don't go through the normal flow.
            InjectContext(SharedContext);
        }

        protected override void OnGUIContentForState()
        {
            GUILayout.Label($"Player Loaded: {SharedContext.TryGet(out PlayerDataWrapper _)}");
            GUILayout.Label($"Party Loaded: {SharedContext.TryGet(out PartyDataWrapper _)}");
        }

        protected override void OnInject()
        {
            UpdateButtons(State);

            State.OptionsChangedSignal.Listen(UpdateButtons);
        }

        protected override void OnDispose()
        {
            _playButton.onClick.RemoveAllListeners();

            State.OptionsChangedSignal.Unlisten(UpdateButtons);
        }

        private void UpdateButtons(ITDHState state)
        {
            if (State.CurrentOptions.TryGetValue(StartupState.CATEGORY_CONTINUE, out var continueOptions))
            {
                if (continueOptions.Count != 1)
                    throw new ApplicationException($"Incorrect number of {StartupState.CATEGORY_CONTINUE} category options!");

                _playButton.SetText(continueOptions[0].Text);
                _playButton.enabled = !continueOptions[0].Disabled;
                _playButton.onClick.RemoveAllListeners();
                _playButton.onClick.AddListener(() => continueOptions[0].SelectOption?.Invoke());
                _playButton.UpdateActive(true);
            }
            else
            {
                _playButton.UpdateActive(false);
            }
        }
    }
}