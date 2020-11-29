using NUnit.Framework;
using Unity.Editor.DataDrawers;
using UnityEngine;
using SadPumpkin.Util.CombatEngine.CostCalculators;

namespace Unity.Editor.Tests
{
    [TestFixture]
    public class DataDrawerFinderTests
    {
        [Test]
        public void finder_finds_cost_drawer()
        {
            var drawer = DataDrawerFinder.Find<ICostCalc>();

            Assert.IsNotNull(drawer);
            Assert.IsInstanceOf<CostDataDrawer>(drawer);
        }
    }
}