using NUnit.Framework;
using CavemanLand.Models.TileSubClasses;
using System;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class MineralsUnitTestClass
    {
        [Test()]
        public void MineralCorrectlyCreated()
        {
			double oceanPer = 1.0;
			double hillPer = .50;
			Minerals minerals1 = new Minerals(oceanPer, hillPer);

			Assert.True(minerals1.surface.ContainsKey("Stone"), "Surface Minerals 1 contains the key Stone");
			Assert.True(minerals1.mineable.ContainsKey("Stone"), "Mineable Minerals 1 contains the key Stone");
			Assert.AreEqual(minerals1.surface["Stone"], 0.0, "Surface Stone is 0.0 in the ocean.");
			Assert.AreEqual(minerals1.surface["Stone"], 0.0, "Mineable Stone is 0.0 in the ocean.");

            
			oceanPer = 0.0;
            hillPer = .50;
			Minerals minerals2 = new Minerals(oceanPer, hillPer);
                     
            Assert.True(minerals2.surface.ContainsKey("Stone"), "Surface Minerals 2 contains the key Stone");
            Assert.True(minerals2.mineable.ContainsKey("Stone"), "Mineable Minerals 2 contains the key Stone");
			Assert.GreaterOrEqual(minerals1.surface["Stone"], 0.0, "Surface Stone is >= 0.0 on the land.");
			Assert.GreaterOrEqual(minerals1.surface["Stone"], 0.0, "Mineable Stone is >= 0.0 on the ocean.");
        }

		[Test()]
        public void SetMineralsForNewMinerals()
        {
			Minerals minerals = new Minerals(0.0, 0.50);
			minerals.setMinerals("Cadmium", 100.0, 1000.0);

			Assert.AreEqual(minerals.surface["Cadmium"], 100.0, "Surface mineral is set with new mineral correctly");
			Assert.AreEqual(minerals.mineable["Cadmium"], 1000.0, "Surface mineral is set with new mineral correctly");
        }

		[Test()]
        public void SetMineralsForOldMinerals()
        {
			Minerals minerals = new Minerals(0.0, 0.50);
            minerals.setMinerals("Stone", 100.0, 1000.0);

            Assert.AreEqual(minerals.surface["Stone"], 100.0, "Surface mineral is set with new mineral correctly");
            Assert.AreEqual(minerals.mineable["Stone"], 1000.0, "Surface mineral is set with new mineral correctly");
        }
    }
}
