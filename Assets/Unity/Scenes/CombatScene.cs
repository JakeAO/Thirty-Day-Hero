using System;
using System.Linq;
using Core.Actors;
using Core.States;
using Core.States.Combat.GameState;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat;
using Unity.Scenes.Combat.ActionSelection;
using Unity.Scenes.Combat.Battlefield;
using Unity.Scenes.Combat.Results;
using Unity.Scenes.Shared.Entities;
using Unity.Scenes.Shared.Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase<CombatState>
    {
        [SerializeField] private Image _battleBack;
        [SerializeField] private EnemyPanel _enemyPanel;
        [SerializeField] private PlayerPanel _playerPanel;
        [SerializeField] private InitiativePanel _initiativePanel;
        [SerializeField] private ResultsPanel _resultsPanel;

        public IUXEventQueue UxEventQueue { get; private set; } = new UXEventQueue();
        public IEventQueue EventQueue { get; private set; }
        public IUnityPool UnityPool { get; private set; }
        public HighlightManager HighlightManager { get; private set; }
        public ActionSelectionWorkflow ActionSelectionWorkflow { get; private set; }

        private EventDataConverter _eventDataConverter = null;

        protected override void OnInject()
        {
            HighlightManager = new HighlightManager(
                SharedContext.Get<IGameState>(),
                _playerPanel,
                _enemyPanel,
                _initiativePanel);
            SharedContext.Set(HighlightManager);

            ActionSelectionWorkflow = new ActionSelectionWorkflow(
                State.PlayerController,
                _playerPanel,
                HighlightManager,
                SharedContext.Get<IGameState>(),
                _playerPanel,
                _enemyPanel);
            SharedContext.Set(ActionSelectionWorkflow);

            EventQueue = State.EventQueue;

            // Pull context data
            UnityPool = SharedContext.Get<IUnityPool>();

            // Setup context data
            SharedContext.Set(UxEventQueue);
            SharedContext.Set(State.EventQueue);
            SharedContext.Set<IActorViewManager<CombatEnemyView>>(_enemyPanel);
            SharedContext.Set<IActorViewManager<CombatPlayerView>>(_playerPanel);
            SharedContext.Set(_initiativePanel);
            SharedContext.Set(_resultsPanel);

            _eventDataConverter = new EventDataConverter(SharedContext);

            // Setup scene elements
            foreach (var actor in State.PartyData.Characters)
            {
                _playerPanel.CreateView(actor);
            }

            foreach (var actor in State.CombatSettings.Enemies)
            {
                _enemyPanel.CreateView(actor);
            }

            _initiativePanel.UpdateInitiativePreview(State.InitiativeQueue
                .GetPreview(10)
                .Select(x => State.GameData.GetActor(x))
                .Where(x => x != null)
                .ToArray());
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            // Tick uxEvent queue
            UxEventQueue.TickUpdate(deltaTime);

            // Convert events to uxEvents
            if (State.EventQueue.TryDequeueEvent(out IEventData eventData))
            {
                foreach (IUXEvent uxEvent in _eventDataConverter.ConvertEvent(eventData))
                {
                    UxEventQueue.AddEvent(uxEvent);
                }
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            SharedContext.Clear<HighlightManager>();
            SharedContext.Clear<ActionSelectionWorkflow>();
            SharedContext.Clear<IUXEventQueue>();
            SharedContext.Clear<IEventQueue>();
            SharedContext.Clear<IActorViewManager<CombatEnemyView>>();
            SharedContext.Clear<IActorViewManager<CombatPlayerView>>();
            SharedContext.Clear<InitiativePanel>();
            SharedContext.Clear<ResultsPanel>();

            // Teardown scene elements
            foreach (var actor in State.PartyData.Characters)
            {
                _playerPanel.DeleteView(actor.Id);
            }

            foreach (var actor in State.CombatSettings.Enemies)
            {
                _enemyPanel.DeleteView(actor.Id);
            }

            _initiativePanel.UpdateInitiativePreview(Array.Empty<ICharacterActor>());
        }
    }
}