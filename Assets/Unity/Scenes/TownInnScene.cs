using Core.Actors.Player;
using Core.States.Town;
using Core.Wrappers;
using UnityEngine;

namespace Unity.Scenes
{
    public class TownInnScene : SceneRootBase<TownInnState>
    {
        private PartyDataWrapper _partyData;

        protected override void OnInject()
        {
            _partyData = SharedContext.Get<PartyDataWrapper>();
        }

        protected override void OnGUIContentForState()
        {
            if (State.RestoredStats != null)
            {
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