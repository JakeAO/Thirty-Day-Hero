using System.Collections.Generic;
using Core.Classes.Player;
using Core.EquipMap;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Actors.Player
{
    public class PlayerCharacter : CharacterActor, IPlayerCharacterActor
    {
        public new IPlayerClass Class
        {
            get => base.Class as IPlayerClass;
            set => base.Class = value;
        }
        public IEquipMap Equipment { get; set; }

        public PlayerCharacter()
            : this(
                0u,
                0u,
                "InvalidPlayer",
                NullPlayerClass.Instance,
                NullStatMap.Instance,
                NullEquipMap.Instance)
        {
        }

        public PlayerCharacter(
            uint id,
            uint party,
            string name,
            IPlayerClass @class,
            IStatMap stats,
            IEquipMap equipment)
            : base(id, party, name, @class, stats)
        {
            Equipment = equipment;
        }

        public override IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IAction> actions = new List<IAction>(base.GetAllActions(possibleTargets));

            if (Equipment != null)
            {
                actions.AddRange(Equipment.GetAllActions(this, possibleTargets));
            }

            return actions;
        }

        public override float GetReducedDamage(float damageAmount, DamageType damageType)
        {
            float finalDamage = base.GetReducedDamage(damageAmount, damageType);

            if (Equipment?.Armor != null)
            {
                finalDamage = Equipment.Armor.GetReducedDamage(finalDamage, damageType);
            }

            return finalDamage;
        }

        public override IInitiativeActor Copy()
        {
            return new PlayerCharacter(
                Id,
                Party,
                Name,
                Class,
                Stats.Copy(),
                Equipment.Copy());
        }
    }
}