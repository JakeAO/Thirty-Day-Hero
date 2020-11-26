using System.Collections.Generic;
using Core.Etc;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

using ActionUtil = Core.Actions.ActionUtil;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.Items.Weapons
{
    public class Weapon : IWeapon
    {
        public uint Id { get; }
        public string Name { get; }
        public string Desc { get; }
        public ItemType ItemType { get; }
        public WeaponType WeaponType { get; }
        public IAbility AttackAbility { get; }
        public IReadOnlyCollection<IAbility> AddedAbilities { get; }

        public Weapon()
            : this(0,
                string.Empty, string.Empty,
                WeaponType.Invalid,
                null,
                null)
        {
        }

        public Weapon(
            uint id,
            string name, string desc,
            WeaponType weaponType,
            IAbility attackAbility,
            IReadOnlyCollection<IAbility> addedAbilities)
        {
            Id = id;
            Name = name;
            Desc = desc;
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