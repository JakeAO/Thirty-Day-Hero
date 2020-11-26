using Core.EquipMap;

namespace Core.Actors
{
    public interface IPlayerCharacterActor : ICharacterActor
    {
        IEquipMap Equipment { get; }
    }
}