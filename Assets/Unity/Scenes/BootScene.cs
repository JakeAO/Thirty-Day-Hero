using System;
using System.IO;
using Core.Etc;
using Core.States;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.StateMachine;
using UnityEngine;

namespace Unity.Scenes
{
    public class BootScene : SceneRootBase
    {
        private void Start()
        {
            // Create context
            Context = new Context();

            // Create and add PathUtility
            PathUtility pathUtility = new PathUtility(
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Ability"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "PlayerClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "EnemyClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "CalamityClass"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "EnemyGroup"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Item"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Weapon"),
                Path.Combine(Application.streamingAssetsPath, "Definitions", "Armor"));
            Context.Set(pathUtility);

            // Create and add state machine
            IStateMachine stateMachine = new StateMachine(Context);
            Context.Set(stateMachine);

            // Initialize StartupState
            stateMachine.ChangeState<StartupState>();
        }

        protected override void OnInject()
        {
            throw new InvalidOperationException($"{nameof(BootScene)} cannot be injected into, it is supposed to be the very first state!");
        }

        private Rect _debugRect = new Rect(10, 10, 150, 150);
        private void OnGUI()
        {
            GUILayout.BeginArea(_debugRect, GUI.skin.box);
            {
                GUILayout.Label("Debug flow:");
                GUILayout.Label("Do you have a party?");
                GUILayout.BeginHorizontal();
                {
                    if(GUILayout.Button("Yes"))
                    {
                        UnityEngine.Debug.Log("USE PARTY DATA, LOAD GAME HUB");
                    }

                    if(GUILayout.Button("No"))
                    {
                        UnityEngine.Debug.Log("LOAD PARTY CREATION");
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}