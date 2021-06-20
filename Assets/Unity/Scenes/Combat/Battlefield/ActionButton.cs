using System;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.Action;
using TMPro;
using Unity.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Battlefield
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _actionSourceIcon;
        [SerializeField] private TMP_Text _actionNameLabel;

        private IAction _action;
        private Action<IAction> _clicked;
        private bool _isFinalActionSelection;

        public void SetAction(IAction action, Action<IAction> clicked, bool isFinalActionSelection)
        {
            _action = action;
            _clicked = clicked;
            _isFinalActionSelection = isFinalActionSelection;
            
            // Button
            _button.enabled = action.Available;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnButtonClicked);

            // Text
            if (action.ActionSource is INamed nameSource)
            {
                _actionNameLabel.text = nameSource.Name;
            }
            else
            {
                _actionNameLabel.text = "???";
            }

            // Image
            if (action.ActionProvider is IIconProvider iconSource)
            {
                Addressables.LoadAssetAsync<Sprite>(iconSource.ArtPath).Completed += OnIconLoaded;
            }
            else
            {
                _actionSourceIcon.sprite = null;
                _actionSourceIcon.gameObject.UpdateActive(false);
            }
        }

        private void OnDisable()
        {
            _actionNameLabel.text = string.Empty;
            _actionSourceIcon.sprite = null;
            _button.onClick.RemoveAllListeners();
        }

        private void OnButtonClicked()
        {
            _clicked?.Invoke(_action);
        }

        private void OnIconLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _actionSourceIcon.sprite = handle.Result;
            _actionSourceIcon.gameObject.UpdateActive(true);
        }
    }
}