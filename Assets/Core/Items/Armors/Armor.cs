using System;
using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

using ActionUtil = Core.Actions.ActionUtil;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items.Armors
{
    public class Armor : IArmor
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ArtPath { get; set; }
        public uint BaseValue { get; set; }
        public RarityCategory Rarity { get; set; }
        public ItemType ItemType { get; set; }
        public ArmorType ArmorType { get; set; }
        public IReadOnlyDictionary<DamageType, float> DamageModifiers { get; set; }
        public IReadOnlyCollection<IAbility> AddedAbilities { get; set; }

        public Armor()
            : this(0u,
                string.Empty,
                string.Empty,
                string.Empty,
                0u,
                RarityCategory.Invalid,
                ArmorType.Invalid,
                null,
                null)
        {
        }

        public Armor(
            uint id,
            string name,
            string desc,
            string artPath,
            uint baseValue,
            RarityCategory rarity,
            ArmorType armorType,
            IReadOnlyDictionary<DamageType, float> damageModifiers,
            IReadOnlyCollection<IAbility> addedAbilities)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ArtPath = artPath;
            BaseValue = baseValue;
            Rarity = rarity;
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