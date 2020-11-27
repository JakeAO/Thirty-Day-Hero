using Core.Classes.Player;
using Core.EquipMap;

namespace Core.Actors.Player
{
    public interface IPlayerCharacterActor : ICharacterActor
    {
        new IPlayerClass Class { get; }
        IEquipMap Equipment { get; }
    }
}