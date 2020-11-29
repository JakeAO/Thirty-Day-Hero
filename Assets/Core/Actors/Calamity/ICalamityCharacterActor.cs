using Core.Classes.Calamity;

namespace Core.Actors.Calamity
{
    public interface ICalamityCharacterActor : ICharacterActor
    {
        new ICalamityClass Class { get; }
    }
}