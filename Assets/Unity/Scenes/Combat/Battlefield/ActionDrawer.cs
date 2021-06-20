using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using Unity.Extensions;
using UnityEngine;

namespace Unity.Scenes.Combat.Battlefield
{
    public class ActionDrawer : MonoBehaviour
    {
        [SerializeField] private ActionButton _buttonPrefab;
        [SerializeField] private RectTransform _listRoot;

        private readonly List<ActionButton> _actionButtons = new List<ActionButton>(10);

        private Action<IAction> _clicked;

        public void SetActions(IReadOnlyList<(IAction, bool)> actions, Action<IAction> clicked)
        {
            _clicked = clicked;
            for (int i = 0; i < actions.Count; i++)
            {
                while (_actionButtons.Count <= i)
                {
                    _actionButtons.Add(Instantiate(_buttonPrefab, _listRoot));
                }

                _actionButtons[i].SetAction(actions[i].Item1, OnClicked, actions[i].Item2);
                _actionButtons[i].gameObject.UpdateActive(true);
            }
        }

        private void OnDisable()
        {
            _clicked = null;
            foreach (ActionButton actionButton in _actionButtons)
            {
                actionButton.gameObject.UpdateActive(false);
            }
        }

        private void OnClicked(IAction action)
        {
            _clicked?.Invoke(action);
        }
    }
}