using System.Collections.Generic;
using Core.Abilities;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

using ActionUtil = Core.Actions.ActionUtil;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items.Weapons
{
    public class Weapon : IWeapon
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ArtPath { get; set; }
        public uint BaseValue { get; set; }
        public RarityCategory Rarity { get; set; }
        public ItemType ItemType { get; set; }
        public WeaponType WeaponType { get; set; }
        public IAbility AttackAbility { get; set; }
        public IReadOnlyCollection<IAbility> AddedAbilities { get; set; }

        public Weapon()
            : this(0u,
                string.Empty,
                string.Empty,
                string.Empty,
                0u,
                RarityCategory.Invalid,
                WeaponType.Invalid,
                null,
                null)
        {
        }

        public Weapon(
            uint id,
            string name,
            string desc,
            string artPath,
            uint baseValue,
            RarityCategory rarity,
            WeaponType weaponType,
            IAbility attackAbility,
            IReadOnlyCollection<IAbility> addedAbilities)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ArtPath = artPath;
            BaseValue = baseValue;
            Rarity = rarity;
            ItemType = ItemType.Weapon;
            WeaponType = weaponType;
            AttackAbility = attackAbility;
            AddedAbilities = addedAbilities != null
                ? new List<IAbility>(addedAbilities)
                : new List<IAbility>();
        }

        public IReadOnlyCollection<IAction> GetAllActions(ICharacterActor sourceCharacter,
            IReadOnlyCollection<ITargetableActor> possibleTargets, bool isEquipped)
        {
            List<IAction> actions = new List<IAction>(10);

            if (AttackAbility != null && isEquipped)
            {
                actions.AddRange(
                    ActionUtil.GetActionsFromAbility(sourceCharacter, this, AttackAbility, possibleTargets));
            }

            if (AddedAbilities != null)
            {
                foreach (IAbility ability in AddedAbilities)
                {
                    actions.AddRange(ActionUtil.GetActionsFromAbility(sourceCharacter, this, ability, possibleTargets));
                }
            }

            return actions;
        }
    }
}