using Core.States;
using UnityEngine;

namespace Unity.Scenes
{
    public class PatrolScene : SceneRootBase<PatrolState>
    {
        protected override void OnGUIContentForState()
        {
            GUILayout.Label(State.EnemyGroup.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
            GUILayout.Label(State.EnemyGroup.Desc);
            GUILayout.Label($"Difficulty: {State.Difficulty}");
        }
    }
}