using NUnit.Framework;
using CavemanLand.Controllers;
using CavemanLand.Models;
using CavemanLand.Models.TileSubClasses;
using CavemanLand.Utility;
using System;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class HabitatsUnitTestClass
    {
		private const int WORLDX = 50;
        private const int WORLDZ = 50;
        World world;
        
		[SetUp()]
        public void Init()
        {
			WorldController worldController = new WorldController();
            world = worldController.generateWorld(50, 50);
        }

		[Test()]
        public void FullHabitatsTest()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
                    Assert.AreEqual(tileArray[x, z].habitats.calculatePercentEmpty(), 0, "tile (" + x + ", " + z + ") is not full.");
                    Assert.AreEqual(tileArray[x, z].habitats.typePercents[13], (int)(tileArray[x, z].terrain.oceanPercent * 100), "tile (" + x + ", " + z + ") oceanPercent and habitat percent that is ocean don't match");
                }
            }
        }
        
        [Test()]
        public void HabitatsAreWithinRange()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
                    assertBetween(tileArray[x, z].habitats.currentLevel, 0.0, 10.0);
                    assertBetween(tileArray[x, z].habitats.gameCurrentLevel, 0.0, 10.0);
                    for (int i = 0; i < tileArray[x, z].habitats.typePercents.Length; i++)
                    {
                        assertBetween(tileArray[x, z].habitats.typePercents[i], 0, 100);
                    }
                }
            }
        }
         
        [Test()]
        public void GetGrazingTest()
        {
			Tile[,] tileArray = world.getTileArray();
			double[,] grazing25 = new double[WORLDX, WORLDZ];
			double[,] grazing50 = new double[WORLDX, WORLDZ];
			double[] grazingHistory = new double[WorldDate.DAYS_PER_YEAR];

 			for (int x = 0; x < WORLDX; x++)
			{
				for (int z = 0; z < WORLDZ; z++)
				{
					for (int day = 0; day < WorldDate.DAYS_PER_YEAR; day++)
					{
						// Setup
						int todaysTemp = tileArray[x, z].temperatures.dailyTemps.days[day];
						double grass;
						double last5Rain = 0.0;
						int firstDay = Math.Max(0, day - 4);
						for (int d = firstDay; d <= day; d++)
						{
							last5Rain += tileArray[x, z].precipitation.dailyRain.precip[d] + tileArray[x, z].rivers.dailyVolume.volume[d] * Habitats.RIVER_WATERING_MULTIPLIER;
						}
						double todaysGrazing = tileArray[x, z].habitats.getGrazing(last5Rain, todaysTemp, out grass);
						// expect to be greater than or equal to 0.0
						Assert.GreaterOrEqual(todaysGrazing, 0.0);
						// expect to be 0.0 when Ocean
						if (tileArray[x, z].terrain.oceanPercent.Equals(1.0))
						{
							Assert.AreEqual(todaysGrazing, 0.0);
						}
                        // If lots of plains there should be some grass.
						if (tileArray[x, z].habitats.typePercents[1] + tileArray[x, z].habitats.typePercents[5] + tileArray[x, z].habitats.typePercents[9] > 50)
                        {
                            Assert.Greater(todaysGrazing, 0.0);
                        }
						// expect to be 0.0 if no desert or plains habitats                  
						int grassedHabitats = sumGrassedHabitats(tileArray[x, z].habitats.typePercents);
						if (grassedHabitats == 0){
							Assert.AreEqual(todaysGrazing, 0.0);
						}
						if (day == 24){
							grazing25[x, z] = todaysGrazing;
						}
						if (day == 49)
                        {
							grazing50[x, z] = todaysGrazing;                     
                        }
						if (x == 25 && z == 25){
							grazingHistory[day] = todaysGrazing;
						}
					}                  
                }
            }

			// print grazing in world on day 25 and day 75
			Console.WriteLine("\nGrazing Day 25: ");
			printArray<double>(grazing25);
			Console.WriteLine("Grazing Day 50: ");
			printArray<double>(grazing50);
			Console.WriteLine("(26, 26) - grazing history: ");
			printArray<double>(grazingHistory);
        }

        [Test()]
        public void GetTreesTest()
        {
			Tile[,] tileArray = world.getTileArray();
            double[,] trees25 = new double[WORLDX, WORLDZ];
            double[,] trees50 = new double[WORLDX, WORLDZ];
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
                    for (int day = 0; day < WorldDate.DAYS_PER_YEAR; day++)
                    {
                        // Setup
                        double todaysTrees = tileArray[x, z].habitats.getTrees();
                        // expect to be greater than or equal to 0.0
						Assert.GreaterOrEqual(todaysTrees, 0);
                        // expect to be 0.0 when Ocean
                        if (tileArray[x, z].terrain.oceanPercent.Equals(1.0))
                        {
							Assert.AreEqual(todaysTrees, 0);
                        }
						// If lots of forest there should be some grass.
                        if (tileArray[x, z].habitats.typePercents[2] + tileArray[x, z].habitats.typePercents[6] + tileArray[x, z].habitats.typePercents[10] > 50)
                        {
                            Assert.Greater(todaysTrees, 0.0);
                        }
                        // expect to be 0.0 if no desert or plains habitats                  
                        int treedHabitats = sumTreedHabitats(tileArray[x, z].habitats.typePercents);
						if (treedHabitats == 0)
                        {
							Assert.AreEqual(todaysTrees, 0.0);
                        }
                        if (day == 24)
                        {
							trees25[x, z] = todaysTrees;
                        }
                        if (day == 49)
                        {
							trees50[x, z] = todaysTrees;
                        }
                    }
                }
            }
            // print grazing in world on day 25 and day 75
            Console.WriteLine("\nTrees Day 25: ");
            printArray<double>(trees25);
            Console.WriteLine("Trees Day 50: ");
            printArray<double>(trees50);
        }
         
		 [Test()]
        public void GetSeedsTest()
        {
			Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					// expect to be greater than or equal to 0.0
					// expect to be 0.0 when Ocean
                    // print seeds in world
                }
            }
        }

        [Test()]
        public void GetFoilageTest()
        {
			Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					// expect to be greater than or equal to 0.0
					// expect to be 0.0 when Ocean
                    // print foilage in world
                }
            }
        }

        [Test()]
        public void GetGatherablesTest()
        {
			Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					// expect to be greater than or equal to 0.0
					// expect to be 0.0 when Ocean
                    // print gatherables in world
                }
            }
        }

        [Test()]
        public void GetGameTest()
        {
			Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					// ???
                }
            }
        }
         
        private void printArray<T>(T[,] array)
        {
            string output = "";
            for (int z = 0; z < WORLDZ; z++)
            {
                for (int x = 0; x < WORLDX; x++)
                {
                    output += array[x, z] + "\t";
                }
                output += "\n";
            }
            Console.Write(output);
        }

		private void printArray<T>(T[] array)
        {
            string output = "";
            for (int i = 0; i < WorldDate.DAYS_PER_YEAR; i++)
            {
                output += array[i] + ", ";
            }
            Console.WriteLine(output);
        }
        
        private void assertBetween(double value, double lowerBound, double higherBound)
        {
            Assert.LessOrEqual(value, higherBound, "Value [" + value + "] is above higherBound [" + higherBound + "]");
            Assert.GreaterOrEqual(value, lowerBound, "Value [" + value + "] is below lowerBound [" + lowerBound + "]");
        }

        private void assertBetween(double value, double lowerBound, double higherBound, string message)
        {
            Assert.LessOrEqual(value, higherBound, "Value [" + value + "] is above higherBound [" + higherBound + "] - " + message);
            Assert.GreaterOrEqual(value, lowerBound, "Value [" + value + "] is below lowerBound [" + lowerBound + "] - " + message);
        }
         
        private int sumGrassedHabitats(int[] typePercents){
			int sum = 0;
			for (int i = 0; i < 12; i+= 4){
				sum += typePercents[i];
				sum += typePercents[i + 1];
			}
			return sum;
		}

		private int sumTreedHabitats(int[] typePercents)
        {
            int sum = 0;
            for (int i = 0; i < 12; i += 4)
            {
                sum += typePercents[i + 2];
                sum += typePercents[i + 3];
            }
            return sum;
        }
        
    }
}
