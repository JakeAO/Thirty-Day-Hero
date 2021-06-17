using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Calamity;
using Core.Actors.Enemy;
using Core.Actors.Player;
using Unity.Scenes.Shared.Pooling;
using UnityEngine;

namespace Unity.Scenes.Shared.Entities
{
    public class ActorViewManager : IActorViewManager
    {
        private readonly IUnityPool _unityPool = null;
        private readonly PlayerActorView _playerPrefab = null;
        private readonly EnemyActorView _enemyPrefab = null;
        private readonly CalamityActorView _calamityPrefab = null;

        private readonly Dictionary<uint, IActorView> _actorViews = new Dictionary<uint, IActorView>(10);

        public ActorViewManager(
            IUnityPool unityPool,
            PlayerActorView playerViewPrefab,
            EnemyActorView enemyViewPrefab,
            CalamityActorView calamityViewPrefab)
        {
            _unityPool = unityPool;
            _playerPrefab = playerViewPrefab;
            _enemyPrefab = enemyViewPrefab;
            _calamityPrefab = calamityViewPrefab;
        }

        public IActorView CreateView(ICharacterActor actorData, Transform parentTransform)
        {
            if (_actorViews.TryGetValue(actorData.Id, out IActorView existingView))
            {
                existingView.UpdateModel(actorData);
                return existingView;
            }

            IActorView newView = null;
            switch (actorData)
            {
                case IPlayerCharacterActor _:
                {
                    newView = _unityPool.Pop(_playerPrefab, parentTransform);
                    break;
                }
                case IEnemyCharacterActor _:
                {
                    newView = _unityPool.Pop(_enemyPrefab, parentTransform);
                    break;
                }
                case ICalamityCharacterActor _:
                {
                    newView = _unityPool.Pop(_calamityPrefab, parentTransform);
                    break;
                }
            }

            if (newView != null)
            {
                newView.InitializeModel(actorData);
                _actorViews[actorData.Id] = newView;
            }

            return newView;
        }

        public bool TryGetView(uint actorId, out IActorView actorView) => _actorViews.TryGetValue(actorId, out actorView);

        public bool TryGetView(ICharacterActor actorData, out IActorView actorView) => TryGetView(actorData.Id, out actorView);

        public void UpdateView(ICharacterActor actorData)
        {
            if (TryGetView(actorData, out IActorView existingView))
            {
                existingView.UpdateModel(actorData);
            }
        }

        public void DeleteView(uint actorId)
        {
            if (TryGetView(actorId, out IActorView existingView))
            {
                _actorViews.Remove(actorId);

                switch (existingView)
                {
                    case PlayerActorView playerActorView:
                        _unityPool.Push(playerActorView);
                        break;
                    case EnemyActorView enemyActorView:
                        _unityPool.Push(enemyActorView);
                        break;
                    case CalamityActorView calamityActorView:
                        _unityPool.Push(calamityActorView);
                        break;
                    case UnityEngine.Component component:
                        _unityPool.Push(component);
                        break;
                }
            }
        }

        public void DeleteView(ICharacterActor actorData) => DeleteView(actorData.Id);
    }
}