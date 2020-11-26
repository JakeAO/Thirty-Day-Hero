using System.Collections.Generic;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.EquipMap
{
    public class EquipMap : IEquipMap
    {
        public IWeapon Weapon { get; set; }
        public IArmor Armor { get; set; }
        public IItem ItemA { get; set; }
        public IItem ItemB { get; set; }

        public IItem this[EquipmentSlot slot]
        {
            get
            {
                switch (slot)
                {
                    case EquipmentSlot.Weapon: return Weapon;
                    case EquipmentSlot.Armor: return Armor;
                    case EquipmentSlot.ItemA: return ItemA;
                    case EquipmentSlot.ItemB: return ItemB;
                }

                return null;
            }
            set
            {
                switch (slot)
                {
                    case EquipmentSlot.Weapon:
                        Weapon = value as IWeapon;
                        break;
                    case EquipmentSlot.Armor:
                        Armor = value as IArmor;
                        break;
                    case EquipmentSlot.ItemA:
                        ItemA = value;
                        break;
                    case EquipmentSlot.ItemB:
                        ItemB = value;
                        break;
                }
            }
        }

        public EquipMap()
            : this(null, null, null, null)
        {
        }

        public EquipMap(
            IWeapon weapon,
            IArmor armor,
            IItem itemA,
            IItem itemB)
        {
            Weapon = weapon;
            Armor = armor;
            ItemA = itemA;
            ItemB = itemB;
        }

        public IReadOnlyCollection<IAction> GetAllActions(ICharacterActor activeChar, IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IAction> allActions = new List<IAction>(10);
            if (Weapon != null)
            {
                allActions.AddRange(Weapon.GetAllActions(activeChar, possibleTargets, true));
            }

            if (Armor != null)
            {
                allActions.AddRange(Armor.GetAllActions(activeChar, possibleTargets, true));
            }

            if (ItemA != null)
            {
                allActions.AddRange(ItemA.GetAllActions(activeChar, possibleTargets, false));
            }

            if (ItemB != null)
            {
                allActions.AddRange(ItemB.GetAllActions(activeChar, possibleTargets, false));
            }

            return allActions;
        }

        public IEquipMap Copy()
        {
            return new EquipMap(
                Weapon,
                Armor,
                ItemA,
                ItemB);
        }
    }
}