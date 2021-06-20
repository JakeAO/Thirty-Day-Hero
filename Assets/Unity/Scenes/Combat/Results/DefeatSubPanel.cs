using System;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scenes.Combat.Results
{
    public class DefeatSubPanel : MonoBehaviour
    {
        [SerializeField] private Button _doneButton;

        private Action _done;

        public void Show(Action done)
        {
            _done = done;
        }

        private void OnDoneClicked()
        {
            _done?.Invoke();
        }

        private void OnEnable()
        {
            _doneButton.onClick.AddListener(OnDoneClicked);
        }

        private void OnDisable()
        {
            _doneButton.onClick.RemoveAllListeners();
        }
    }
}