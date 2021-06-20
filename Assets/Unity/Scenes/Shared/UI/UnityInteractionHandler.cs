using System;
using Unity.Scenes.Combat.Etc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.Scenes.Shared.UI
{
    public class UnityInteractionHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _requiredTimeForHold = 2f;

        public event Action<InteractionType> Interacted;

        private float? _lastPointerDown;

        private void OnEnable()
        {
            _lastPointerDown = null;
        }

        private void OnDisable()
        {
            _lastPointerDown = null;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _lastPointerDown = null;
        }

        private void Update()
        {
            if (Interacted == null ||
                _requiredTimeForHold <= 0f ||
                _lastPointerDown == null)
                return;

            float heldTime = Time.timeSinceLevelLoad - _lastPointerDown.Value;
            if (heldTime >= _requiredTimeForHold)
            {
                Interacted?.Invoke(InteractionType.Hold);
                _lastPointerDown = null;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                {
                    Interacted?.Invoke(InteractionType.LeftClick);
                    eventData.Use();
                    break;
                }
                case PointerEventData.InputButton.Right:
                {
                    Interacted?.Invoke(InteractionType.RightClick);
                    eventData.Use();
                    break;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _lastPointerDown = Time.realtimeSinceStartup;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _lastPointerDown = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Interacted?.Invoke(InteractionType.Hover);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _lastPointerDown = null;
            Interacted?.Invoke(InteractionType.None);
        }
    }
}