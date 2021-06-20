using System;
using System.Collections.Generic;
using Core.Actors;
using Core.Actors.Player;
using SadPumpkin.Util.CombatEngine.Action;
using Unity.Extensions;
using Unity.Scenes.Shared.Entities;
using UnityEngine;

namespace Unity.Scenes.Combat.Battlefield
{
    public class PlayerPanel : MonoBehaviour, IActorViewManager<CombatPlayerView>, IActionPromptDisplay
    {
        [SerializeField] private CombatPlayerView _playerViewPrefab;
        [SerializeField] private RectTransform _playerParent;
        [SerializeField] private ActionDrawer _playerActionDrawer;

        private readonly List<CombatPlayerView> _playerViews = new List<CombatPlayerView>(10);

        public CombatPlayerView CreateView(ICharacterActor actorData)
        {
            if (!(actorData is IPlayerCharacterActor))
            {
                throw new ArgumentException($"{nameof(PlayerPanel)} can only accept player actors, but was provided \"{actorData.GetType().Name}\"", nameof(actorData));
            }

            var view = Instantiate(_playerViewPrefab, _playerParent); // TODO pooling
            view.InitializeModel(actorData);

            _playerViews.Add(view);

            return view;
        }

        public bool TryGetView(uint actorId, out IActorView actorView)
        {
            foreach (var view in _playerViews)
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
            foreach (var view in _playerViews)
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
            foreach (var view in _playerViews)
            {
                if (view.Model.Id == actorId)
                {
                    _playerViews.Remove(view);
                    Destroy(view); // TODO pooling
                    return;
                }
            }
        }

        public void ShowActionPrompt(uint actorId, IReadOnlyList<(IAction, bool)> actions, Action<IAction> clicked)
        {
            if (TryGetView(actorId, out IActorView actorView) &&
                actorView is CombatPlayerView combatView)
            {
                Vector3 localPosition = _playerActionDrawer.transform.localPosition;
                localPosition.x = combatView.transform.localPosition.x;
                _playerActionDrawer.transform.localPosition = localPosition;

                _playerActionDrawer.SetActions(actions, clicked);
                _playerActionDrawer.UpdateActive(true);
            }
        }

        public void HideActionPrompt()
        {
            _playerActionDrawer.UpdateActive(false);
        }
    }
}