using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.States.Combat;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.GameState;
using System.Collections.Generic;
using System.Linq;
using Unity.Scenes.Combat;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase<CombatMainState>
    {
        [SerializeField] private LayoutGroup _playerLayoutGroup;
        [SerializeField] private PlayerCharacterCombatPane _playerCombatPanePrefab;
        [SerializeField] private LayoutGroup _enemyLayoutGroup;
        [SerializeField] private EnemyCharacterCombatPane _enemyCombatPanePrefab;
        
        private IGameState _currentGameState => State.CurrentGameState;

        private Vector2 _initiativeScroll = new Vector2(0, 0);
        private readonly Dictionary<string, Texture2D> _enemyArtTextures = new Dictionary<string, Texture2D>();

        private readonly Dictionary<uint, PlayerCharacterCombatPane> _playerPanes = new Dictionary<uint, PlayerCharacterCombatPane>();
        private readonly Dictionary<uint, EnemyCharacterCombatPane> _enemyPanes = new Dictionary<uint, EnemyCharacterCombatPane>();
        
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

            SetupPlayerCharacters(State.PartyData.Characters);
            SetupEnemyCharacters(State.Settings.Enemies);
        }

        private void SetupPlayerCharacters(IReadOnlyCollection<IPlayerCharacterActor> playerCharacters)
        {
            foreach (IPlayerCharacterActor character in playerCharacters)
            {
                var pane = Instantiate(_playerCombatPanePrefab, _playerLayoutGroup.transform);
                pane.UpdateCharacter(character);
                _playerPanes[character.Id] = pane;
            }
        }

        private void SetupEnemyCharacters(IReadOnlyCollection<IEnemyCharacterActor> enemyCharacters)
        {
            foreach (IEnemyCharacterActor character in enemyCharacters)
            {
                var pane = Instantiate(_enemyCombatPanePrefab, _enemyLayoutGroup.transform);
                pane.UpdateCharacter(character);
                _enemyPanes[character.Id] = pane;
            }
        }

        private void Update()
        {
            if (State.GameStateDirtied)
            {
                foreach (var initiativePair in State.CurrentGameState.InitiativeOrder)
                {
                    switch (initiativePair.Entity)
                    {
                        case IPlayerCharacterActor playerCharacterActor:
                            if (_playerPanes.TryGetValue(playerCharacterActor.Id, out var playerPane))
                                playerPane.UpdateCharacter(playerCharacterActor);
                            break;
                        case IEnemyCharacterActor enemyCharacterActor:
                            if (_enemyPanes.TryGetValue(enemyCharacterActor.Id, out var enemyPane))
                                enemyPane.UpdateCharacter(enemyCharacterActor);
                            break;
                    }
                }

                State.GameStateDirtied = false;
            }
        }

        protected override void OnGUIContentForState()
        {
            RenderInitiative(_currentGameState.InitiativeOrder);
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
                            GUILayout.Label(pair.Entity.Name, GUILayout.Height(20));
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

        private Color ActorColor(IInitiativeActor actor)
        {
            uint activeActorId = _currentGameState.ActiveActor != null ? _currentGameState.ActiveActor.Id : 0;

            if (!actor.IsAlive())
            {
                if (actor.Id == activeActorId)
                {
                    return Color.Lerp(Color.green, Color.red, 0.5f);
                }
                else
                {
                    return Color.red;
                }
            }
            else if (actor.Id == activeActorId)
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