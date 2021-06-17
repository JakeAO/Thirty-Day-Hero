using Core.Classes.Calamity;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Actors.Calamity
{
    public class CalamityCharacter : CharacterActor, ICalamityCharacterActor
    {
        public new ICalamityClass Class
        {
            get => base.Class as ICalamityClass;
            set => base.Class = value;
        }

        public CalamityCharacter()
            : this(
                0u,
                0u,
                "InvalidEnemy",
                NullCalamityClass.Instance,
                NullStatMap.Instance)
        {
        }

        public CalamityCharacter(
            uint id,
            uint party,
            string name,
            ICalamityClass @class,
            IStatMap stats)
            : base(id, party, name, @class, stats)
        {
        }

        public override IInitiativeActor Copy()
        {
            return new CalamityCharacter(
                Id,
                Party,
                Name,
                Class,
                Stats.Copy());
        }
    }
}