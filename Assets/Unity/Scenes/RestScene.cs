using Core.Actors.Player;
using Core.Etc;
using Core.States;
using Core.Wrappers;
using UnityEngine;

namespace Unity.Scenes
{
    public class RestScene : SceneRootBase<RestState>
    {
        private PartyDataWrapper _partyData;
        private TimeOfDay _timeOfDay;

        protected override void OnInject()
        {
            base.OnInject();

            _partyData = SharedContext.Get<PartyDataWrapper>();
            _timeOfDay = _partyData.Time;
        }

        protected override void OnGUIContentForState()
        {
            if (State.RestoredStats == null)
            {
                GUILayout.Label("The party makes camp and sits down for a meal.", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
            }
            else
            {
                switch (_timeOfDay)
                {
                    case TimeOfDay.Night:
                        GUILayout.Label("The party has a restful night's sleep at camp and wakes feeling refreshed.", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                        break;
                    default:
                        GUILayout.Label("The party gets what rest they can while the sun is still up.", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
                        break;
                }

                foreach (PlayerCharacter playerCharacter in _partyData.Characters)
                {
                    State.RestoredStats.TryGetValue(playerCharacter.Id, out (uint hp, uint sta) restored);

                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label(playerCharacter.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                        GUILayout.Label($"HP +{restored.hp}, STA +{restored.sta}");
                    }
                    GUILayout.EndVertical();
                }
            }
        }
    }
}