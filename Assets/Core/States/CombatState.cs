using System.Collections.Generic;
using System.Linq;
using Core.Actions;
using Core.CharacterControllers;
using Core.CombatSettings;
using Core.Etc;
using Core.EventOptions;
using Core.States.BaseClasses;
using Core.States.Combat;
using Core.States.Combat.Events;
using Core.States.Combat.GameState;
using Core.Wrappers;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.CombatEngine.Initiatives;
using SadPumpkin.Util.CombatEngine.TurnController;
using SadPumpkin.Util.CombatEngine.WinningPartyCalculator;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.StateMachine;
using SadPumpkin.Util.StateMachine.States;

namespace Core.States
{
    public class CombatState : TDHStateBase
    {
        public const string CATEGORY_DEFAULT = "";
        public const string CATEGORY_DEBUG = "";

        public CombatSettings.CombatSettings CombatSettings { get; private set; }

        public PartyDataWrapper PartyData { get; private set; }
        public PlayerCharacterController PlayerController { get; private set; }
        public IInitiativeQueue InitiativeQueue { get; private set; }
        public IEventQueue EventQueue { get; private set; }
        public CombatManager CombatManager { get; private set; }
        public CombatResults Results { get; private set; }
        public GameState GameData { get; private set; }

        private IEventQueue _internalEventQueue = new EventQueue();

        public CombatState(CombatSettings.CombatSettings combatSettings)
        {
            CombatSettings = combatSettings;
        }

        public override void OnEnter(IState fromState)
        {
            base.OnEnter(fromState);

            // Initialize all our pieces
            PartyData = SharedContext.Get<PartyDataWrapper>();
            PlayerController = new PlayerCharacterController(PartyData);
            InitiativeQueue = new InitiativeQueue(100);
            EventQueue = new EventQueue();
            CombatManager = new CombatManager(
                new[]
                {
                    PartyData.GetAsParty(PlayerController),
                    CombatSettings.GetAsParty()
                },
                new OneActionTurnController(),
                new StandardActionGenerator(),
                new AnyAliveWinningPartyCalculator(),
                new ActorChangeCalculator(),
                InitiativeQueue,
                _internalEventQueue);
            GameData = new GameState(
                PartyData.Characters,
                CombatSettings.Enemies);

            // Add necessary items to Context
            SharedContext.Set<IInitiativeQueue>(InitiativeQueue);
            SharedContext.Set<IGameState>(GameData);
        }

        public override void OnContent()
        {
            base.OnContent();

            // Start that combat
            CombatManager.Start(true);
        }

        public override void OnExit(IState toState)
        {
            base.OnExit(toState);

            SharedContext.Clear<IInitiativeQueue>();
            SharedContext.Clear<IGameState>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            // Process events from internal queue
            if (_internalEventQueue.TryDequeueEvent(out IEventData eventData))
            {
                // Pass out event for potential view-layer handling
                EventQueue.EnqueueEvent(eventData);

                // Handle event internally
                ProcessEventData(eventData);
            }
        }

        private void ProcessEventData(IEventData eventData)
        {
            switch (eventData)
            {
                case CombatCompletedEvent cce:
                {
                    Results = cce.WinningPartyId == PartyData.PartyId
                        ? CombatResults.CreateSuccess(CombatSettings.Enemies, PartyData)
                        : CombatResults.CreateFailure();
                    EventQueue.EnqueueEvent(new CombatResultsEvent(Results, OnCombatResultsConfirmed));
                    break;
                }
                case ActiveActorChangedEvent aace:
                {
                    GameData.SetActiveActor(GameData.GetActor(aace.NewActorId));

                    UpdateOptions();
                    break;
                }
                case ActorActionTakenEvent aate:
                {
                    UpdateOptions();
                    break;
                }
            }
        }

        private void OnCombatResultsConfirmed()
        {
            IStateMachine stateMachine = SharedContext.Get<IStateMachine>();
            if (Results.Success)
            {
                if (CombatSettings.Enemies.Contains(PartyData.Calamity))
                {
                    stateMachine.ChangeState<VictoryState>();
                }
                else
                {
                    stateMachine.ChangeState<GameHubState>();
                }
            }
            else
            {
                stateMachine.ChangeState<DefeatState>();
            }
        }

        private void UpdateOptions()
        {
            _currentOptions.Clear();

            if (!_currentOptions.TryGetValue(CATEGORY_DEBUG, out var debugList))
                _currentOptions[CATEGORY_DEBUG] = debugList = new List<IEventOption>(2);

            debugList.Add(new EventOption(
                "Win Combat",
                () =>
                {
                    foreach (var enemy in CombatSettings.Enemies)
                    {
                        enemy.Stats.ModifyStat(StatType.HP, -(int) enemy.Stats[StatType.HP_Max]);
                    }
                },
                CATEGORY_DEBUG));
            debugList.Add(new EventOption(
                "Lose Combat",
                () =>
                {
                    foreach (var pc in PartyData.Characters)
                    {
                        pc.Stats.ModifyStat(StatType.HP, -(int) pc.Stats[StatType.HP_Max]);
                    }
                },
                CATEGORY_DEBUG));

            OptionsChangedSignal?.Fire(this);
        }
    }
}