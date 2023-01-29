using System;
using SadPumpkin.Util.Context;

namespace Unity.SceneController
{
    public interface ISceneRoot : IDisposable
    {
        IContext SharedContext { get; }

        void InjectContext(IContext context);
    }
}
