using Core.Actors;

namespace Unity.Scenes.Shared.Entities
{
    public interface IActorView
    {
        ICharacterActor Model { get; }
        void InitializeModel(ICharacterActor actorData);
        void UpdateModel(ICharacterActor actorData, ActorUpdateContext context);
    }
}