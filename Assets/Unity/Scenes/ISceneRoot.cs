using System;
using SadPumpkin.Util.Context;

namespace Unity.Scenes
{
    public interface ISceneRoot : IDisposable
    {
        IContext Context { get; }

        void InjectContext(IContext context);
    }
}
