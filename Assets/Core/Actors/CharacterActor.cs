using System;
using System.Collections.Generic;
using Core.Abilities;
using Core.Actions;
using Core.Classes;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace Core.Actors
{
    public abstract class CharacterActor : ICharacterActor
    {
        public uint Id { get; set; }
        public uint Party { get; set; }
        public string Name { get; set; }
        public IClass Class { get; set; }
        public IStatMap Stats { get; set; }

        public bool IsAlive() => Stats[StatType.HP] > 0u;
        public bool CanTarget() => IsAlive();

        public float GetInitiative() => 10f + (Stats[StatType.DEX] - 10f - 2 * (Stats[StatType.LVL] - 1)) / Stats[StatType.LVL];

        protected CharacterActor(
            uint id,
            uint party,
            string name,
            IClass @class,
            IStatMap stats)
        {
            Id = id;
            Party = party;
            Name = name;
            Class = @class;
            Stats = stats;
        }

        public virtual IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IAction> actions = new List<IAction>(10);

            foreach (IAbility ability in Class.GetAllAbilities(Stats[StatType.LVL]))
            {
                actions.AddRange(ActionUtil.GetActionsFromAbility(this, Class, ability, possibleTargets));
            }

            return actions;
        }

        public virtual float GetReducedDamage(float damageAmount, DamageType damageType)
        {
            if (Class.IntrinsicDamageModification != null && Class.IntrinsicDamageModification.Count > 0)
            {
                float damage = damageAmount;
                if (Class.IntrinsicDamageModification.TryGetValue(damageType, out float modifier))
                {
                    damage *= modifier;
                }
                else
                {
                    foreach (int enumValue in Enum.GetValues(typeof(DamageType)))
                    {
                        DamageType enumType = (DamageType) enumValue;
                        if ((enumValue & (int) damageType) == enumValue &&
                            Class.IntrinsicDamageModification.TryGetValue(enumType, out modifier))
                        {
                            damage *= modifier;
                        }
                    }
                }

                return damage;
            }

            return damageAmount;
        }

        public abstract IInitiativeActor Copy();
    }
}