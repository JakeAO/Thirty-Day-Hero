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
        private bool _inventoryToggle = false;
        
        protected override void OnGUIContentForState()
        {
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            GUILayout.Label($"Party Id: {partyDataWrapper.PartyId}");
            GUILayout.Label($"Calamity: {partyDataWrapper.Calamity.Name} ({partyDataWrapper.Calamity.Class.Name})");
            GUILayout.Label($"Day: {partyDataWrapper.Day} ({partyDataWrapper.Time})");
            GUILayout.Label($"Gold: {partyDataWrapper.Gold}");
            GUILayout.Label("Party:", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
            GUILayout.BeginHorizontal();
            {
                foreach (PlayerCharacter character in partyDataWrapper.Characters)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label(character.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                        GUILayout.Label($"Lvl {character.Stats[StatType.LVL]} {character.Class.Name}");
                        GUILayout.Label($"HP [{character.Stats[StatType.HP]} / {character.Stats[StatType.HP_Max]}]");
                        GUILayout.Label($"STA [{character.Stats[StatType.STA]} / {character.Stats[StatType.STA_Max]}]");
                        GUILayout.Label($"XP [{character.Stats[StatType.EXP]} / 100]");
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();

            _inventoryToggle = GUILayout.Toggle(_inventoryToggle, "Inventory:", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
            if (_inventoryToggle)
            {
                foreach (IItem item in partyDataWrapper.Inventory)
                {
                    GUILayout.Label($"    {item.Name}");
                }
            }
        }
    }
}