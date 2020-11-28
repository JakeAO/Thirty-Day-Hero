using Core.Actors.Enemy;
using Core.CombatSettings;
using Core.States.Combat;
using SadPumpkin.Util.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase
    {
        private Rect _debugRect = new Rect(10, 10, 150, 150);
        private void OnGUI()
        {
            GUILayout.BeginArea(_debugRect, GUI.skin.box);
            {
                GUILayout.Label("Debug flow:");
                if (GUILayout.Button("Win (Enemy)"))
                {
                    CombatResults win = CombatResults.CreateSuccess(
                        new List<IEnemyCharacterActor>(),
                        new Core.Wrappers.PartyDataWrapper());

                    Context.Get<IStateMachine>().ChangeState(new CombatEndState(win));
                }

                if (GUILayout.Button("Win (Bossfight)"))
                {
                    CombatResults win = CombatResults.CreateSuccess(
                        new List<IEnemyCharacterActor>(),
                        new Core.Wrappers.PartyDataWrapper());

                    Context.Get<IStateMachine>().ChangeState(new CombatEndState(win));
                }

                if (GUILayout.Button("Lose"))
                {
                    CombatResults lose = CombatResults.CreateFailure();

                    Context.Get<IStateMachine>().ChangeState(new CombatEndState(lose));
                }
            }
            GUILayout.EndArea();
        }
    }
}
