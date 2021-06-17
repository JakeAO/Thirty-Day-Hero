using Core.EquipMap;
using Core.Etc;

namespace Core.Classes.Player
{
    public interface IPlayerClass : IClass
    {
        string ArtPath { get; }
        WeaponType WeaponProficiency { get; }
        ArmorType ArmorProficiency { get; }
        IEquipMapBuilder StartingEquipment { get; }
    }
}