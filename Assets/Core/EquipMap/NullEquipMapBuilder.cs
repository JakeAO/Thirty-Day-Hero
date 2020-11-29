﻿using System;

namespace Core.EquipMap
{
    public class NullEquipMapBuilder : IEquipMapBuilder
    {
        public static readonly NullEquipMapBuilder Instance = new NullEquipMapBuilder();

        private NullEquipMapBuilder()
        {
            
        }
        
        public IEquipMap Generate(Random random)
        {
            return new EquipMap();
        }
    }
}