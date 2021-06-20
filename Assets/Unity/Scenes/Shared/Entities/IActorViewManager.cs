using Core.Actors;
using UnityEngine;

namespace Unity.Scenes.Shared.Entities
{
    public interface IActorViewManager<out T> where T : MonoBehaviour
    {
        T CreateView(ICharacterActor actorData);

        bool TryGetView(uint actorId, out IActorView actorView);
        
        void UpdateView(ICharacterActor actorData);

        void DeleteView(uint actorId);
    }
}