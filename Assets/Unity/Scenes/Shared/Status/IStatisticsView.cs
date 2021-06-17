using Core.Actors;

namespace Unity.Scenes.Shared.Status
{
    public interface IStatisticsView
    {
        void UpdateModel(ICharacterActor actorData);
    }
}