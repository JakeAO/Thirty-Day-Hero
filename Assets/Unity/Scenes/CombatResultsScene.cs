using System;
using Core.Actors.Player;
using Core.Etc;
using Core.States.Combat;
using Core.Wrappers;
using UnityEngine;

namespace Unity.Scenes
{
    public class CombatResultsScene : SceneRootBase<CombatEndState>
    {
        private PartyDataWrapper _partyData;

        protected override void OnInject()
        {
            _partyData = SharedContext.Get<PartyDataWrapper>();
        }

        protected override void OnGUIContentForState()
        {
            if (State.Victory)
            {
                GUILayout.Label($"Gold Reward: {State.GoldReward}");
                GUILayout.Label($"Exp Reward: {State.ExpPerCharacter}");

                foreach (PlayerCharacter playerCharacter in _partyData.Characters)
                {
                    if (State.StatChanges.TryGetValue(playerCharacter.Id, out var statChange))
                    {
                        GUILayout.BeginVertical(GUI.skin.box);
                        {
                            GUILayout.Label(playerCharacter.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                            GUILayout.BeginHorizontal(GUI.skin.box);
                            {
                                foreach (StatType statType in Enum.GetValues(typeof(StatType)))
                                {
                                    if (statType == StatType.Invalid ||
                                        statChange[(int) statType] <= 0)
                                        continue;

                                    GUILayout.BeginVertical();
                                    GUILayout.Label(statType.ToString(), new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                                    GUILayout.Label($"+{statChange[(int) statType]}");
                                    GUILayout.EndVertical();
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                }
            }
            else
            {
                GUILayout.Label("The party was defeated and the calamity will rage across the kingdom.", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Italic});
            }
        }
    }
}