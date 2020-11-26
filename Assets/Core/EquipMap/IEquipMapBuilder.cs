using System;

namespace Core.EquipMap
{
    public interface IEquipMapBuilder
    {
        IEquipMap Generate(Random random);
    }
}