using Core.Actors.Enemy;
using Core.Etc;
using Core.States.Combat;
using UnityEngine;

namespace Unity.Scenes
{
    public class CombatSetupScene : SceneRootBase<CombatSetupState>
    {
        protected override void OnGUIContentForState()
        {
            GUILayout.Label("Enemy Party", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
            GUILayout.BeginVertical(GUI.skin.box);
            {
                foreach (IEnemyCharacterActor enemy in State.CombatSettings.Enemies)
                {
                    GUILayout.Label(enemy.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                    GUILayout.Label($"Lvl {enemy.Stats[StatType.LVL]} {enemy.Class.Name}");
                }
            }
            GUILayout.EndVertical();
        }
    }
}