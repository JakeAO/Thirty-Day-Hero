using Core.Actors;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.Etc;
using Core.States.Combat;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.GameState;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase<CombatMainState>
    {
        private IGameState _currentGameState => State.CurrentGameState;

        protected override void OnGUIContentForState()
        {
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            RenderInitiative(_currentGameState.InitiativeOrder);
            RenderEnemies(State.Settings.Enemies);
            RenderPartyCharacters(partyDataWrapper.Characters);
        }

        private Vector2 _initiativeScroll = new Vector2(0, 0);
        private void RenderInitiative(IEnumerable<IInitiativePair> initiativePairs)
        {
            GUILayout.Label("Initiative:");
            
            _initiativeScroll = GUILayout.BeginScrollView(_initiativeScroll, GUI.skin.box, GUILayout.Width(500));
            {
                GUILayout.BeginHorizontal();
                {
                    foreach (InitiativePair pair in initiativePairs)
                    {
                        TextAnchor prvAnchor = GUI.skin.label.alignment;
                        GUI.skin.label.alignment = TextAnchor.MiddleCenter;

                        GUILayout.BeginVertical(GUI.skin.box);
                        {
                            GUI.color = ActorColor(pair.Entity);
                            GUILayout.Label(pair.Entity.Name, GUILayout.Height(40));
                            GUI.color = Color.white;
                        }
                        GUILayout.EndVertical();

                        GUI.skin.label.alignment = prvAnchor;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void RenderEnemies(IEnumerable<IEnemyCharacterActor> enemies)
        {
            GUILayout.Label("Enemies:");
            RenderActors(enemies);
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
            GUI.color = ActorColor(actor);
            GUILayout.Label($"    {actor.Name} (Lvl {actor.Stats[StatType.LVL]} {actor.Class.Name})\n" +
                                $"       HP: {actor.Stats[StatType.HP]}/{actor.Stats[StatType.HP_Max]} | STA {actor.Stats[StatType.STA]}/{actor.Stats[StatType.STA_Max]}");
            GUI.color = Color.white;
        }

        private UnityEngine.Color ActorColor(IInitiativeActor actor)
        {
            uint activeActorId = _currentGameState.ActiveActor != null ? _currentGameState.ActiveActor.Id : 0;

            if (actor.Id == activeActorId)
            {
                return Color.green;
            }
            else
            {
                return Color.white;
            }
        }
    }
}