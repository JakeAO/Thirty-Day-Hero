using Core.Actors;
using Core.Actors.Player;
using Core.States;
using SadPumpkin.Util.Events;
using SadPumpkin.Util.UXEventQueue;
using Unity.Scenes.Combat;
using Unity.Scenes.Shared.Entities;
using Unity.Scenes.Shared.Pooling;
using Unity.Scenes.Shared.Status;
using UnityEngine;

namespace Unity.Scenes
{
    public class CombatScene : SceneRootBase<CombatState>
    {
        [SerializeField] private Transform _playerRow;
        [SerializeField] private Transform _enemyRow;

        public IUXEventQueue UxEventQueue { get; private set; } = new UXEventQueue();
        public IEventQueue EventQueue { get; private set; }
        public IUnityPool UnityPool { get; private set; }
        public IActorViewManager ActorViewManager { get; private set; }
        public IStatusViewManager StatusViewManager { get; private set; }

        private EventDataConverter _eventDataConverter = null;

        protected override void OnInject()
        {
            EventQueue = State.EventQueue;

            // Pull context data
            UnityPool = SharedContext.Get<IUnityPool>();
            ActorViewManager = SharedContext.Get<IActorViewManager>();
            StatusViewManager = SharedContext.Get<IStatusViewManager>();

            // Setup context data
            SharedContext.Set(UxEventQueue);
            SharedContext.Set(State.EventQueue);

            _eventDataConverter = new EventDataConverter(SharedContext);

            // Setup scene elements
            foreach (ICharacterActor actor in State.CombatData.AllActors)
            {
                switch (actor)
                {
                    case IPlayerCharacterActor _:
                        ActorViewManager.CreateView(actor, _playerRow);
                        break;
                    default:
                        ActorViewManager.CreateView(actor, _enemyRow);
                        break;
                }
            }
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

            SharedContext.Clear<IUXEventQueue>();
            SharedContext.Clear<IEventQueue>();
            
            // Teardown scene elements
            foreach (ICharacterActor actor in State.CombatData.AllActors)
            {
                ActorViewManager.DeleteView(actor);
            }
        }
    }
}