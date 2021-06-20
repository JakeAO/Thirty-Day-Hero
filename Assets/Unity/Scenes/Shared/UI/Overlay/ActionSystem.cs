using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Scenes.Shared.UI.Overlay
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    public class ActionSystem : MonoBehaviour
    {

        private IContext _context;

        public void Inject(IContext activeContext)
        {
            _context = activeContext;
        }
    }
}
