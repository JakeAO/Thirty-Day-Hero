using System;

namespace Core.Etc
{
    [Flags]
    public enum WeaponType
    {
        Invalid = 0,
        
        Sword = 1,
        GreatSword = 2,
        Axe = 4,
        GreatAxe = 8,
        Spear = 16,
        Staff = 32,
        Rod = 64,
        Bow = 128,
        Fist = 256
    }
}