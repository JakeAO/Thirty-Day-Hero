using SadPumpkin.Util.Context;
using UnityEngine;

namespace Unity.Scenes
{
    public abstract class SceneRootBase : MonoBehaviour, ISceneRoot
    {

        public IContext Context { get; }
        public void InjectContext(IContext context)
        {
            throw new System.NotImplementedException();
        }
    
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
