using Core.Actors;
using UnityEngine;

namespace Unity.Scenes.Shared.Entities
{
    public interface IActorViewManager
    {
        IActorView CreateView(ICharacterActor actorData, Transform parentTransform);

        bool TryGetView(uint actorId, out IActorView actorView);
        bool TryGetView(ICharacterActor actorData, out IActorView actorView);
        
        void UpdateView(ICharacterActor actorData);

        void DeleteView(uint actorId);
        void DeleteView(ICharacterActor actorData);
    }
}