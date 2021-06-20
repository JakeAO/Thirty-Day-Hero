using System;
using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Calamity;
using Core.Actors.Enemy;
using Unity.Scenes.Shared.Entities;
using UnityEngine;

namespace Unity.Scenes.Combat.Battlefield
{
    public class EnemyPanel : MonoBehaviour, IActorViewManager<CombatEnemyView>
    {
        [SerializeField] private CombatEnemyView _enemyViewPrefab;
        [SerializeField] private RectTransform _enemyParent;

        private readonly List<CombatEnemyView> _enemyViews = new List<CombatEnemyView>(10);

        public CombatEnemyView CreateView(ICharacterActor actorData)
        {
            if (!(actorData is IEnemyCharacterActor) &&
                !(actorData is ICalamityCharacterActor))
            {
                throw new ArgumentException($"{nameof(EnemyPanel)} can only accept enemy and calamity actors, but was provided \"{actorData.GetType().Name}\"", nameof(actorData));
            }

            var view = Instantiate(_enemyViewPrefab, _enemyParent); // TODO pooling
            view.InitializeModel(actorData);

            _enemyViews.Add(view);

            return view;
        }

        public bool TryGetView(uint actorId, out IActorView actorView)
        {
            foreach (var view in _enemyViews)
            {
                if (view.Model.Id == actorId)
                {
                    actorView = view;
                    return true;
                }
            }

            actorView = null;
            return false;
        }

        public void UpdateView(ICharacterActor actorData)
        {
            foreach (var view in _enemyViews)
            {
                if (view.Model.Id == actorData.Id)
                {
                    view.UpdateModel(actorData, ActorUpdateContext.DEFAULT);
                    return;
                }
            }
        }

        public void DeleteView(uint actorId)
        {
            foreach (var view in _enemyViews)
            {
                if (view.Model.Id == actorId)
                {
                    _enemyViews.Remove(view);
                    Destroy(view); // TODO pooling
                    return;
                }
            }
        }
    }
}