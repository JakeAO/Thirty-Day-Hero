using Core.States;
using Core.States.Town;
using SadPumpkin.Util.StateMachine;
using UnityEngine;

namespace Unity.Scenes
{
    public class GameHubScene : SceneRootBase
    {
        private Rect _debugRect = new Rect(10, 10, 150, 150);
        private void OnGUI()
        {
            GUILayout.BeginArea(_debugRect, GUI.skin.box);
            {
                GUILayout.Label("Debug flow:");
                //Town
                if (GUILayout.Button("To Town"))
                {
                    Context.Get<IStateMachine>().ChangeState<TownHubState>();
                }

                //Combat
                if(GUILayout.Button("Go on patrol"))
                {
                    Context.Get<IStateMachine>().ChangeState<PatrolState>();
                }

                //Encounter
                if(GUILayout.Button("[Special Event]"))
                {
                    UnityEngine.Debug.Log("NAV TO EVENT");
                }
            }
            GUILayout.EndArea();
        }
    }
}
