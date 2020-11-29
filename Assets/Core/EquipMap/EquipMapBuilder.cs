using System;
using System.Collections.Generic;
using Core.Etc;
using Core.Items;
using Core.Items.Armors;
using Core.Items.Weapons;

namespace Core.EquipMap
{
    public class EquipMapBuilder : IEquipMapBuilder
    {
        public List<(IWeapon, RankPriority)> StartingWeapon { get; set; }
        public List<(IArmor, RankPriority)> StartingArmor { get; set; }
        public List<(IItem, RankPriority)> StartingItemA { get; set; }
        public List<(IItem, RankPriority)> StartingItemB { get; set; }

        public EquipMapBuilder()
        {
            StartingWeapon = new List<(IWeapon, RankPriority)>();
            StartingArmor = new List<(IArmor, RankPriority)>();
            StartingItemA = new List<(IItem, RankPriority)>();
            StartingItemB = new List<(IItem, RankPriority)>();
        }

        public EquipMapBuilder(
            IReadOnlyCollection<(IWeapon, RankPriority)> startingWeapon,
            IReadOnlyCollection<(IArmor, RankPriority)> startingArmor,
            IReadOnlyCollection<(IItem, RankPriority)> startingItemA,
            IReadOnlyCollection<(IItem, RankPriority)> startingItemB
        )
        {
            StartingWeapon = new List<(IWeapon, RankPriority)>(startingWeapon);
            StartingArmor = new List<(IArmor, RankPriority)>(startingArmor);
            StartingItemA = new List<(IItem, RankPriority)>(startingItemA);
            StartingItemB = new List<(IItem, RankPriority)>(startingItemB);
        }

        public IEquipMap Generate(Random random)
        {
            return new EquipMap(
                GetRandomItem(StartingWeapon, random),
                GetRandomItem(StartingArmor, random),
                GetRandomItem(StartingItemA, random),
                GetRandomItem(StartingItemB, random));
        }

        private static T GetRandomItem<T>(IReadOnlyCollection<(T, RankPriority)> dict, Random rand) where T : IItem
        {
            double totalPriority = 0d;
            foreach ((T _, RankPriority priority) in dict)
            {
                totalPriority += Constants.PRIORITY_WEIGHT[priority];
            }

            double randVal = rand.NextDouble() * totalPriority;
            foreach ((T item, RankPriority priority) in dict)
            {
                randVal -= Constants.PRIORITY_WEIGHT[priority];
                if (randVal <= 0)
                {
                    return item;
                }
            }

            return default;
        }
    }
}