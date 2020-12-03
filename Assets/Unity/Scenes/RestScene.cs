using Core.Actors.Player;
using Core.Etc;
using Core.States;
using UnityEngine;

namespace Unity.Scenes
{
    public class RestScene : SceneRootBase<RestState>
    {
        protected override void OnGUIContentForState()
        {
            switch (State.TimeOfDay)
            {
                case TimeOfDay.Night:
                    GUILayout.Label("The party has a restful night's sleep at camp and wakes feeling refreshed.", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                    break;
                default:
                    GUILayout.Label("The party makes camp while the sun is still up and gets what rest they can.", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                    break;
            }

            foreach (PlayerCharacter playerCharacter in State.PartyData.Characters)
            {
                State.RestoredStats.TryGetValue(playerCharacter.Id, out (uint hp, uint sta) restored);

                GUILayout.Label(playerCharacter.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                GUILayout.Label($"HP +{restored.hp}, STA +{restored.sta}");
            }
        }
    }
}