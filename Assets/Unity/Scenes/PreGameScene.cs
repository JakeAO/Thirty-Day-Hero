using Core.States;
using SadPumpkin.Util.StateMachine;
using UnityEngine;

namespace Unity.Scenes
{
    public class PreGameScene : SceneRootBase
    {
        private Rect _debugRect = new Rect(10, 10, 150, 170);
        private void OnGUI()
        {
            GUILayout.BeginArea(_debugRect, GUI.skin.box);
            {
                GUILayout.Label("Debug flow:");
                if(GUILayout.Button("Continue"))
                {
                    Context.Get<IStateMachine>().ChangeState<GameHubState>();
                }
            }
            GUILayout.EndArea();
        }
    }
}