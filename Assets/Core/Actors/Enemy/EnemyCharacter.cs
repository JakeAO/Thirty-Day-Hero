using Core.Classes.Enemy;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Actors.Enemy
{
    public class EnemyCharacter : CharacterActor, IEnemyCharacterActor
    {
        public new IEnemyClass Class
        {
            get => base.Class as IEnemyClass;
            set => base.Class = value;
        }

        public EnemyCharacter()
            : this(
                0u,
                0u,
                "InvalidEnemy",
                NullEnemyClass.Instance,
                NullStatMap.Instance)
        {
        }

        public EnemyCharacter(
            uint id,
            uint party,
            string name,
            IEnemyClass @class,
            IStatMap stats)
            : base(id, party, name, @class, stats)
        {
        }

        public override IInitiativeActor Copy()
        {
            return new EnemyCharacter(
                Id,
                Party,
                Name,
                Class,
                Stats.Copy());
        }
    }
}