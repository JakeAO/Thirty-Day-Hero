using System;

namespace Core.Etc
{
    [Flags]
    public enum ArmorType
    {
        Invalid = 0,
        
        Light = 1,
        Medium = 2,
        Heavy = 4
    }
}