using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Scenes
{
    public abstract class SceneRootBase : MonoBehaviour, ISceneRoot
    {
        public IContext Context { get; protected set; }

        public void InjectContext(IContext context)
        {
            Context = context;

            OnInject();
        }

        public void Dispose()
        {
            OnDispose();

            Context = null;
        }

        protected virtual void OnInject()
        {
        }

        protected virtual void OnDispose()
        {
        }
    }
}