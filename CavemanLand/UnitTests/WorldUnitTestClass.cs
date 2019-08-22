using NUnit.Framework;
using CavemanLand.Models;
using CavemanLand.Utility;
using System;
using System.Diagnostics;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class WorldUnitTestClass
    {
		private const int WORLDX = 50;
		private const int WORLDZ = 50;
		World world;

		[SetUp()]
		public void Init()
		{
			world = new World(WORLDX, WORLDZ);
		}
        
        [Test()]
        public void gameFileSavingTest()
        {
			world.saveGameFiles();
			// verify that this works correctly.
			verifyExpectedJson(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/UnitTests/Mocks/worldFileMock.json", @"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/SaveFiles/WorldA-worldFile.json");
        }

		[Test()]
        public void generationTimingTest()
        {
			Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
			World world2 = new World(50, 50);
            stopWatch.Stop();
			TimeSpan ts = stopWatch.Elapsed;
			Console.WriteLine("(50, 50) World Creation Time: " + ts);
			stopWatch.Reset();

			stopWatch.Start();
			world2 = new World(100, 100);
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("(100, 100) World Creation Time: " + ts);
			stopWatch.Reset();

			stopWatch.Start();
			world2 = new World(100, 50);
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("(100, 50) World Creation Time: " + ts);
			stopWatch.Reset();

            stopWatch.Start();
			world2 = new World(200, 200);
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("(200, 200) World Creation Time: " + ts);
        }

		[Test()]
        public void verifyOceanPercentageRange()
        {
			Tile[,] tileArray = world.getTileArray();
			for (int x = 0; x < WORLDX; x++){
				for (int z = 0; z < WORLDZ; z++){
					assertBetween(tileArray[x, z].terrain.oceanPercent, 0.0, 1.0);
				}
			}
        }

		[Test()]
        public void verifyHillPercentageRange()
        {
			Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
                    assertBetween(tileArray[x, z].terrain.hillPercent, 0.0, 1.0);
                }
            }
        }

		[Test()]
        public void IfOceanNoHills()
        {
			Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					if (tileArray[x, z].terrain.oceanPercent == 1.0){
						Assert.AreEqual(tileArray[x, z].terrain.hillPercent, 0.0);
					}
                }
            }
        }

        private void verifyExpectedJson(string expectedPath, string actualPath)
		{
			string expectedJson = MyJsonFileInteractor.loadJsonFileToString(expectedPath);
			string actualJson = MyJsonFileInteractor.loadJsonFileToString(actualPath);
			Assert.AreEqual(simplifyJson(expectedJson), actualJson);
			Console.Write("File is correctly saved!");
		}

		// removes tabs and line break characters to make comparing jsons accurate.
        private string simplifyJson(string json)
        {
            string newjson = json.Replace("\t", "");
			newjson = newjson.Replace("\r", "");
            newjson = newjson.Replace("\n", "");
            newjson = newjson.Replace(", ", ",");
            newjson = newjson.Replace(": ", ":");
            return newjson;
        }

		private void assertBetween(double value, double lowerBound, double higherBound)
        {
            Assert.LessOrEqual(value, higherBound, "Value [" + value + "] is above higherBound [" + higherBound + "]");
            Assert.GreaterOrEqual(value, lowerBound, "Value [" + value + "] is below lowerBound [" + lowerBound + "]");
        }
       
    }
}
