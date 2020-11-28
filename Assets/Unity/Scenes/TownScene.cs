using Core.States;
using Core.States.Town;
using SadPumpkin.Util.StateMachine;
using UnityEngine;

namespace Unity.Scenes
{
    public class TownScene : SceneRootBase
    {
        private Rect _debugRect = new Rect(10, 10, 150, 170);
        private void OnGUI()
        {
            GUILayout.BeginArea(_debugRect, GUI.skin.box);
            {
                GUILayout.Label("Debug flow:");
                if (GUILayout.Button("Leave Town"))
                {
                    Context.Get<IStateMachine>().ChangeState<GameHubState>();
                }

                GUILayout.Space(5);

                if(GUILayout.Button("Shop"))
                {
                    Context.Get<IStateMachine>().ChangeState<TownShopState>();
                }

                if(GUILayout.Button("Dojo"))
                {
                    Context.Get<IStateMachine>().ChangeState<TownDojoState>();
                }

                if(GUILayout.Button("Inn"))
                {
                    Context.Get<IStateMachine>().ChangeState<TownInnState>();
                }

                if(GUILayout.Button("[Town Event]"))
                {
                    //idk, spitballing here.  Maybe the town is on fire by some fire elementals
                }
            }
            GUILayout.EndArea();
        }
    }
}
