using Core.Actors.Player;
using Core.Etc;
using Core.States;
using Core.Wrappers;
using UnityEngine;

namespace Unity.Scenes
{
    public class EncounterScene : SceneRootBase<EncounterState>
    {
        protected override void OnGUIContentForState()
        {
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            GUILayout.Label($"Day: {partyDataWrapper.Day} ({partyDataWrapper.Time})");
            GUILayout.Label($"Gold: {partyDataWrapper.Gold}");
            GUILayout.Label("Party:");
            foreach (PlayerCharacter character in partyDataWrapper.Characters)
            {
                GUILayout.Label($"    {character.Name} (Lvl {character.Stats[StatType.LVL]} {character.Class.Name})");
            }
        }
    }
}
