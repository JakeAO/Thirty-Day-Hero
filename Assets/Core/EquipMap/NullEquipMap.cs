using System.Collections.Generic;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.EquipMap
{
    public class NullEquipMap : IEquipMap
    {
        public static NullEquipMap Instance = new NullEquipMap();

        private NullEquipMap()
        {
        }
        
        public IWeapon Weapon => null;
        public IArmor Armor => null;
        public IItem ItemA => null;
        public IItem ItemB => null;

        public IItem this[EquipmentSlot slot] => null;

        public IReadOnlyCollection<IAction> GetAllActions(ICharacterActor activeChar, IReadOnlyCollection<ITargetableActor> possibleTargets) => new IAction[0];
        
        public IEquipMap Copy()
        {
            return Instance;
        }
    }
}