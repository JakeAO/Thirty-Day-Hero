using System.Collections.Generic;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;
using SadPumpkin.Util.CombatEngine;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using ICharacterActor = Core.Actors.ICharacterActor;

namespace Core.EquipMap
{
    public interface IEquipMap : ICopyable<IEquipMap>
    {
        IWeapon Weapon { get; }
        IArmor Armor { get; }
        IItem ItemA { get; }
        IItem ItemB { get; }

        IItem this[EquipmentSlot slot] { get; }
        
        IReadOnlyCollection<IAction> GetAllActions(ICharacterActor activeChar, IReadOnlyCollection<ITargetableActor> possibleTargets);
    }
}