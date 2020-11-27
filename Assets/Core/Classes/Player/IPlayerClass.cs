using Core.EquipMap;
using Core.Etc;

namespace Core.Classes.Player
{
    public interface IPlayerClass : IClass
    {
        WeaponType WeaponProficiency { get; }
        ArmorType ArmorProficiency { get; }
        IEquipMapBuilder StartingEquipment { get; }
    }
}