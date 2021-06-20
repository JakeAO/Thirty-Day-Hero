using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Calamity;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Core.States.Combat.GameState;
using Unity.Scenes.Combat.Etc;
using Unity.Scenes.Shared.Entities;

namespace Unity.Scenes.Combat.Battlefield
{
    public class HighlightManager
    {
        private readonly IGameState _gameState;
        private readonly IActorViewManager<CombatPlayerView> _playerViewManager;
        private readonly IActorViewManager<CombatEnemyView> _enemyViewManager;
        private readonly InitiativePanel _initiativePanel;

        public HighlightManager(
            IGameState gameState,
            IActorViewManager<CombatPlayerView> playerViewManager,
            IActorViewManager<CombatEnemyView> enemyViewManager,
            InitiativePanel initiativePanel)
        {
            _gameState = gameState;
            _playerViewManager = playerViewManager;
            _enemyViewManager = enemyViewManager;
            _initiativePanel = initiativePanel;
        }

        public void UpdateHighlights(IReadOnlyDictionary<uint, HighlightType> highlights)
        {
            foreach (ICharacterActor characterActor in _gameState.AllActors)
            {
                // Calc Highlight
                if (!highlights.TryGetValue(characterActor.Id, out HighlightType highlightType))
                {
                    highlightType = _gameState.ActiveActor?.Id == characterActor.Id ? HighlightType.Active : HighlightType.None;
                }

                // Pull Actor View
                IHighlightableView highlightableView = null;
                switch (characterActor)
                {
                    case IPlayerCharacterActor _:
                        highlightableView = (IHighlightableView) (_playerViewManager.TryGetView(characterActor.Id, out var playerActorView) ? playerActorView : null);
                        break;
                    case IEnemyCharacterActor _:
                    case ICalamityCharacterActor _:
                        highlightableView = (IHighlightableView) (_enemyViewManager.TryGetView(characterActor.Id, out var enemyActorView) ? enemyActorView : null);
                        break;
                }

                // Update Actor View
                if (highlightableView != null)
                {
                    highlightableView.SetHighlight(highlightType);
                }

                // Update Initiative View
                _initiativePanel.HighlightActor(characterActor.Id, highlightType);
            }
        }
    }
}