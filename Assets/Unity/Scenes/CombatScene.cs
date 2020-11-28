using Core.Actors;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.CombatSettings;
using Core.Etc;
using Core.States.Combat;
using Core.Wrappers;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase<CombatMainState>
    {
        protected override void OnGUIContentForState()
        {
            CombatSettings combatSettings = SharedContext.Get<CombatSettings>();
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            RenderEnemies(combatSettings != null ? combatSettings.Enemies : null);
            RenderPartyCharacters(partyDataWrapper.Characters);
        }

        private void RenderEnemies(IEnumerable<IEnemyCharacterActor> enemies)
        {
            GUILayout.Label("Enemies:");
            //RenderActors(enemies);
        }

        private void RenderPartyCharacters(IEnumerable<PlayerCharacter> playerCharacters)
        {
            GUILayout.Label("Party:");
            RenderActors(playerCharacters);
        }

        private void RenderActors(IEnumerable<ICharacterActor> actors)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                foreach (ICharacterActor actor in actors)
                {
                    RenderActor(actor);
                }
            }
            GUILayout.EndVertical();
        }

        private void RenderActor(ICharacterActor actor)
        {
            GUILayout.Label($"    {actor.Name} (Lvl {actor.Stats[StatType.LVL]} {actor.Class.Name})\n" +
                                $"       HP: {actor.Stats[StatType.HP]}/{actor.Stats[StatType.HP_Max]} | STA {actor.Stats[StatType.STA]}/{actor.Stats[StatType.STA_Max]}");
        }
    }
}