using Core.Actors.Player;
using Core.Etc;
using Core.Items;
using Core.States;
using Core.Wrappers;
using UnityEngine;

namespace Unity.Scenes
{
    public class GameHubScene : SceneRootBase<GameHubState>
    {
        protected override void OnGUIContentForState()
        {
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            GUILayout.Label($"Party Id: {partyDataWrapper.PartyId}");
            GUILayout.Label($"Calamity: {partyDataWrapper.Calamity.Name} ({partyDataWrapper.Calamity.Class.Name})");
            GUILayout.Label($"Day: {partyDataWrapper.Day} ({partyDataWrapper.Time})");
            GUILayout.Label($"Gold: {partyDataWrapper.Gold}");
            GUILayout.Label("Party:");
            foreach (PlayerCharacter character in partyDataWrapper.Characters)
            {
                GUILayout.Label($"    {character.Name} (Lvl {character.Stats[StatType.LVL]} {character.Class.Name})");
            }
            GUILayout.Label("Inventory:");
            foreach (IItem item in partyDataWrapper.Inventory)
            {
                GUILayout.Label($"    {item.Name}");
            }
        }
    }
}