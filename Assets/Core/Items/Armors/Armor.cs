using System;
using System.Collections.Generic;
using Core.Etc;

using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

using ActionUtil = Core.Actions.ActionUtil;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items.Armors
{
    public class Armor : IArmor
    {
        public uint Id { get; }
        public string Name { get; }
        public string Desc { get; }
        public ItemType ItemType { get; }
        public ArmorType ArmorType { get; }
        public IReadOnlyDictionary<DamageType, float> DamageModifiers { get; }
        public IReadOnlyCollection<IAbility> AddedAbilities { get; }

        public Armor()
            : this(0,
                string.Empty, String.Empty,
                ArmorType.Invalid,
                null, 
                null)
        {
        }

        public Armor(
            uint id,
            string name, string desc,
            ArmorType armorType,
            IReadOnlyDictionary<DamageType, float> damageModifiers,
            IReadOnlyCollection<IAbility> addedAbilities)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ItemType = ItemType.Armor;
            ArmorType = armorType;
            DamageModifiers = damageModifiers != null
                ? new Dictionary<DamageType, float>((IDictionary<DamageType, float>) damageModifiers)
                : new Dictionary<DamageType, float>();
            AddedAbilities = addedAbilities != null
                ? new List<IAbility>(addedAbilities)
                : new List<IAbility>();
        }

        public IReadOnlyCollection<IAction> GetAllActions(ICharacterActor sourceCharacter,
            IReadOnlyCollection<ITargetableActor> possibleTargets, bool isEquipped)
        {
            List<IAction> actions = new List<IAction>(10);

            if (AddedAbilities != null)
            {
                foreach (IAbility ability in AddedAbilities)
                {
                    actions.AddRange(ActionUtil.GetActionsFromAbility(sourceCharacter, this, ability, possibleTargets));
                }
            }

            return actions;
        }

        public float GetReducedDamage(float damageAmount, DamageType damageType)
        {
            if (DamageModifiers != null && DamageModifiers.Count > 0)
            {
                float damage = damageAmount;
                if (DamageModifiers.TryGetValue(damageType, out float modifier))
                {
                    damage *= modifier;
                }
                else
                {
                    foreach (int enumValue in Enum.GetValues(typeof(DamageType)))
                    {
                        DamageType enumType = (DamageType) enumValue;
                        if ((enumValue & (int) damageType) == enumValue &&
                            DamageModifiers.TryGetValue(enumType, out modifier))
                        {
                            damage *= modifier;
                        }
                    }
                }

                return damage;
            }
            else
            {
                return damageAmount;
            }
        }
    }
}