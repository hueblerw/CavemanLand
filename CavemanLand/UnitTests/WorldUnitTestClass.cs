using NUnit.Framework;
using CavemanLand.Models;
using CavemanLand.Utility;
using System;
using System.Collections.Generic;
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

        // Terrain generation tests
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

        // Temperature Layer generation tests

		[Test()]
        public void TempValuesRangeTest()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
                    assertBetween(tileArray[x, z].temperatures.low, -25, 75);
					assertBetween(tileArray[x, z].temperatures.high, 15, 115);
					assertBetween(tileArray[x, z].temperatures.summerLength, 36, 84);
                    assertBetween(tileArray[x, z].temperatures.variance, 0.0, 12.0);
                }
            }
        }

		[Test()]
        public void HighTempIsHigherThanLowTempTest()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					Assert.GreaterOrEqual(tileArray[x, z].temperatures.high, tileArray[x, z].temperatures.low + 10);
                }
            }
        }

		// Precipitation Layer generation tests

		[Test()]
        public void HumiditiesAreWithinRange()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					Assert.AreEqual(tileArray[x, z].precipitation.humidities.Length, 6);
					for (int h = 0; h < tileArray[x, z].precipitation.humidities.Length; h++){
						assertBetween(tileArray[x, z].precipitation.humidities[h], 0.0, 12.0);
					}
                }
            }
        }

		[Test()]
        public void PrintYearHumidity()
        {
            Tile[,] tileArray = world.getTileArray();
			string output = "";
			int x = 0;
			int z = 0;
			for (int h = 0; h < tileArray[x, z].precipitation.humidities.Length; h++)
            {
                Console.Write(tileArray[x, z].precipitation.humidities[h] + " - ");
            }
			Console.Write("\n");
			for (int d = 1; d < WorldDate.DAYS_PER_YEAR; d++)
			{
				output += tileArray[x, z].precipitation.getHumidity(d) + ", ";
			}
			Console.WriteLine("Humidity Year: ");
			Console.WriteLine(output);
        } 

		[Test()]
        public void GetHumidityFunctionWorks()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					for (int d = 1; d <= WorldDate.DAYS_PER_YEAR - 20; d++)
					{
						double first = tileArray[x, z].precipitation.humidities[(d - 1) / 20];
						double second = tileArray[x, z].precipitation.humidities[((d - 1) / 20) + 1];
						if(first > second){
							assertBetween(tileArray[x, z].precipitation.getHumidity(d), second - .001, first + .001, "Cell (" + x + ", " + z + ") day - " + d);
						} else {
							assertBetween(tileArray[x, z].precipitation.getHumidity(d), first - .001, second + .001, "Cell (" + x + ", " + z + ") day - " + d);
						}                  
					}
					for (int d = WorldDate.DAYS_PER_YEAR - 20 + 1; d <= WorldDate.DAYS_PER_YEAR; d++)
                    {
						double first = tileArray[x, z].precipitation.humidities[(d - 1) / 20];
                        double second = tileArray[x, z].precipitation.humidities[0];
                        if(first > second){
                            assertBetween(tileArray[x, z].precipitation.getHumidity(d), second - .001, first + .001, "Cell (" + x + ", " + z + ") day - " + d);
                        } else {
                            assertBetween(tileArray[x, z].precipitation.getHumidity(d), first - .001, second + .001, "Cell (" + x + ", " + z + ") day - " + d);
                        } 
                    }
                }
            }
        }      

		// River Direction Tests
		[Test()]
        public void FlowRateIsWithinRange()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					Assert.GreaterOrEqual(tileArray[x, z].rivers.flowRate, 0.0);
					if(tileArray[x, z].rivers.flowDirection == Direction.CardinalDirections.none){
						Assert.AreEqual(tileArray[x, z].rivers.flowRate, 0.0);
					}
                }
            }
        }

		[Test()]
        public void DownhillFlowIsCorrect()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					Direction.CardinalDirections direction = tileArray[x, z].rivers.flowDirection;
					Coordinates myPosition = new Coordinates(x, z);
					if (!direction.Equals(Direction.CardinalDirections.none))
					{
						Coordinates downhill = myPosition.findCoordinatesInCardinalDirection(direction);
						Assert.Less(tileArray[downhill.x, downhill.z].terrain.elevation, tileArray[x, z].terrain.elevation, "Cell: " + myPosition + " - downhill: " + downhill);
					}
					else
					{
						List<Direction.CardinalDirections> around = myPosition.getCardinalDirectionsAround();
						foreach (Direction.CardinalDirections dir in around)
						{
							Coordinates coor = myPosition.findCoordinatesInCardinalDirection(dir);
							Assert.LessOrEqual(tileArray[x, z].terrain.elevation, tileArray[coor.x, coor.z].terrain.elevation, "Cell: " + myPosition + " - downhill: " + dir);
						}
					}
                }
            }
        }

		[Test()]
        public void DownhillAndUphillFlowMatch()
        {
            Tile[,] tileArray = world.getTileArray();
            for (int x = 0; x < WORLDX; x++)
            {
                for (int z = 0; z < WORLDZ; z++)
                {
					List<Direction.CardinalDirections> uphills = tileArray[x, z].rivers.upstreamDirections;
					if (uphills.Count > 0)
					{
						Coordinates myPosition = new Coordinates(x, z);
						foreach(Direction.CardinalDirections direction in uphills)
						{
							Coordinates uphill = myPosition.findCoordinatesInCardinalDirection(direction);
							Assert.True(Direction.isOpposite(tileArray[uphill.x, uphill.z].rivers.flowDirection, direction));
						}
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

		private void assertBetween(double value, double lowerBound, double higherBound, string message)
        {
            Assert.LessOrEqual(value, higherBound, "Value [" + value + "] is above higherBound [" + higherBound + "] - " + message);
            Assert.GreaterOrEqual(value, lowerBound, "Value [" + value + "] is below lowerBound [" + lowerBound + "] - " + message);
        }
       
    }
}
