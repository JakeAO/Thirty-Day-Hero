using System;
using System.Collections.Generic;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;

namespace Core.Wrappers
{
    public class TownDataWrapper
    {
        private static readonly Random RANDOM = new Random();

        public bool HasInn { get; }
        public uint InnCost { get; }

        public bool HasDojo { get; }
        public uint DojoCost { get; }

        public bool HasItemShop { get; }
        public float ItemShopCost { get; }
        public List<IItem> ItemShopInventory { get; }

        public bool HasWeaponShop { get; }
        public float WeaponShopCost { get; }
        public List<IWeapon> WeaponShopInventory { get; }

        public bool HasArmorShop { get; }
        public float ArmorShopCost { get; }
        public List<IArmor> ArmorShopInventory { get; }

        public TownDataWrapper()
        {
            HasInn = RANDOM.NextDouble() < 0.75f;
            InnCost = (uint) (50 + RANDOM.Next(50));

            HasDojo = RANDOM.NextDouble() < 0.2f;
            DojoCost = (uint) (150 + RANDOM.Next(150));

            HasItemShop = RANDOM.NextDouble() < 0.75f;
            ItemShopCost = (float) (0.8f + RANDOM.NextDouble() * 0.4f);
            if (HasItemShop)
            {
                // TODO Generate random inventory
            }

            HasWeaponShop = RANDOM.NextDouble() < 0.5f;
            WeaponShopCost = (float) (0.8f + RANDOM.NextDouble() * 0.4f);
            if (HasWeaponShop)
            {
                // TODO Generate random inventory
            }

            HasArmorShop = RANDOM.NextDouble() < 0.5f;
            ArmorShopCost = (float) (0.8f + RANDOM.NextDouble() * 0.4f);
            if (HasArmorShop)
            {
                // TODO Generate random inventory
            }
        }
    }
}