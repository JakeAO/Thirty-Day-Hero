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
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase<CombatMainState>
    {
        private IGameState _currentGameState => State.CurrentGameState;

        private Vector2 _initiativeScroll = new Vector2(0, 0);
        private readonly Dictionary<string, Texture2D> _enemyArtTextures = new Dictionary<string, Texture2D>();

        protected override void OnInject()
        {
            foreach (string artPath in State.Settings.Enemies
                .Select(x => x.Class.ArtPath)
                .Distinct())
            {
                if (!string.IsNullOrWhiteSpace(artPath))
                {
                    string artPathCache = artPath;
                    Addressables.LoadAssetAsync<Texture2D>(artPathCache).Completed += delegate(AsyncOperationHandle<Texture2D> handle)
                    {
                        Texture2D texture2D = handle.Result;
                        if (texture2D)
                            _enemyArtTextures[artPathCache] = handle.Result;
                    };
                }
            }
        }

        protected override void OnGUIContentForState()
        {
            PartyDataWrapper partyDataWrapper = SharedContext.Get<PartyDataWrapper>();

            RenderInitiative(_currentGameState.InitiativeOrder);
            RenderEnemies(State.Settings.Enemies);
            RenderPartyCharacters(partyDataWrapper.Characters);
        }

        private void RenderInitiative(IEnumerable<IInitiativePair> initiativePairs)
        {
            GUILayout.Label("Initiative:");

            _initiativeScroll = GUILayout.BeginScrollView(_initiativeScroll, GUI.skin.box, GUILayout.Width(500));
            {
                GUILayout.BeginHorizontal();
                {
                    foreach (IInitiativePair pair in initiativePairs)
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
            GUILayout.Label("Enemies:", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
            RenderActors(enemies);
        }

        private void RenderPartyCharacters(IEnumerable<PlayerCharacter> playerCharacters)
        {
            GUILayout.Label("Party:", new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
            RenderActors(playerCharacters);
        }

        private void RenderActors(IEnumerable<ICharacterActor> actors)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            {
                foreach (ICharacterActor actor in actors)
                {
                    RenderActor(actor);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void RenderActor(ICharacterActor actor)
        {
            GUI.backgroundColor = ActorColor(actor);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                if (actor is IEnemyCharacterActor enemyActor &&
                    _enemyArtTextures.TryGetValue(enemyActor.Class.ArtPath, out Texture2D artTexture))
                {
                    GUILayout.Label(artTexture, GUILayout.Width(50), GUILayout.Height(50));
                }

                GUILayout.Label(actor.Name, new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold});
                GUILayout.Label($"Lvl {actor.Stats[StatType.LVL]} {actor.Class.Name}");
                GUILayout.Label($"HP [{actor.Stats[StatType.HP]} / {actor.Stats[StatType.HP_Max]}]");
                GUILayout.Label($"STA [{actor.Stats[StatType.STA]} / {actor.Stats[StatType.HP_Max]}]");
            }
            GUILayout.EndVertical();
            GUI.backgroundColor = Color.white;
        }

        private Color ActorColor(IInitiativeActor actor)
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