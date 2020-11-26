using Core.EquipMap;
using Core.Etc;
using Core.Items.Armors;
using Core.Items.Weapons;

namespace Core.Classes
{
    public interface IPlayerClass : ICharacterClass
    {
        WeaponType WeaponProficiency { get; }
        ArmorType ArmorProficiency { get; }
        IEquipMapBuilder StartingEquipment { get; }
    }
}