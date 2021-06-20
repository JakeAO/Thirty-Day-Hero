using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Scenes.Shared.UI.Overlay
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    public class OverlaySystem : MonoBehaviour
    {
        [SerializeField] private PopupSystem _popupSystem;
        [SerializeField] private ActionSystem _actionSystem;
        [SerializeField] private TransitionSystem _transitionSystem;
        [SerializeField] private ToastSystem _toastSystem;

        public PopupSystem PopupSystem => _popupSystem;
        public ActionSystem ActionSystem => _actionSystem;
        public TransitionSystem TransitionSystem => _transitionSystem;
        public ToastSystem ToastSystem => _toastSystem;

        private IContext _context;

        public void Inject(IContext activeContext)
        {
            _context = activeContext;
        }
    }
}