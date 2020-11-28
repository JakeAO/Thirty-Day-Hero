using Core.Actors.Player;
using Core.Etc;
using Core.States.Combat;
using Core.Wrappers;
using UnityEngine;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase<CombatMainState>
    {
        protected override void OnGUIContentForState()
        {
            // TODO Show Enemy Data
            GUILayout.Label("Enemies:");
            GUILayout.Label("TODO ENEMIES GO HERE");

            // Show Party Data
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            GUILayout.Label("Party:");
            foreach (PlayerCharacter character in partyDataWrapper.Characters)
            {
                GUILayout.Label($"    {character.Name} (Lvl {character.Stats[StatType.LVL]} {character.Class.Name})\n" +
                                $"       HP: {character.Stats[StatType.HP]}/{character.Stats[StatType.HP_Max]} | STA {character.Stats[StatType.STA]}/{character.Stats[StatType.STA_Max]}");
            }
        }
    }
}