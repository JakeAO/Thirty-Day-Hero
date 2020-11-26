using System.Collections.Generic;
using Core.EquipMap;
using Core.Etc;
using Core.StatMap;
using SadPumpkin.Util.CombatEngine.Abilities;

namespace Core.Classes
{
    public class PlayerClass : CharacterClass, IPlayerClass
    {
        public WeaponType WeaponProficiency { get; }
        public ArmorType ArmorProficiency { get; }
        public IEquipMapBuilder StartingEquipment { get; }

        public PlayerClass(
            uint id,
            string name,
            string desc,
            IDictionary<DamageType, float> intrinsicDamageModification,
            IStatMapBuilder startingStats,
            IStatMapIncrementor levelUpStats,
            IDictionary<uint, IReadOnlyCollection<IAbility>> abilitiesPerLevel,
            WeaponType weaponProficiency,
            ArmorType armorProficiency,
            IEquipMapBuilder startingEquipment)
            : base(
                id,
                name,
                desc,
                intrinsicDamageModification,
                startingStats,
                levelUpStats,
                abilitiesPerLevel)
        {
            WeaponProficiency = weaponProficiency;
            ArmorProficiency = armorProficiency;
            StartingEquipment = startingEquipment;
        }
    }
}