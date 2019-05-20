using NUnit.Framework;
using System;
using CavemanLand.Generators;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class LayerGeneratorTestClass
    {
        [Test()]
        public void ArrayTest()
        {
			LayerGenerator layerGenerator = new LayerGenerator(60, 50, 2);
			double[,] array = layerGenerator.GenerateWorldLayer(40, 80, 3.0, 45, false);
			printArray(array);
			// Console.WriteLine("Normal 60 x 50 layer generation took: " + stopwatch.Elapsed + "secs.");
			Assert.AreEqual(45.0, array[0, 0]);
			verifyAllValuesInRange(40.0, 80.0, array);
        }
        
		[Test()]
        public void SquaredArrayTest()
        {
            LayerGenerator layerGenerator = new LayerGenerator(60, 50, 2);
            double[,] squaredArray = layerGenerator.GenerateWorldLayer(40, 80, 3.0, 45, true);
			printArray(squaredArray);
			// Console.WriteLine("Squared 60 x 50 layer generation took: " + stopwatch.Elapsed + "secs.");
			Assert.AreEqual(45.0, squaredArray[0, 0]);
			verifyAllValuesInRange(40.0, 80.0, squaredArray);
        }

		[Test()]
        public void IntArrayTest()
        {
            LayerGenerator layerGenerator = new LayerGenerator(60, 50, 0);
            double[,] array = layerGenerator.GenerateWorldLayer(40, 80, 3.0, 45, false);
			int[,] intArray = layerGenerator.convertDoubleArrayToInt(array);
			printArray(intArray);
            // Console.WriteLine("Normal 60 x 50 layer generation took: " + stopwatch.Elapsed + "secs.");
            Assert.AreEqual(45, intArray[0, 0]);
            verifyAllValuesInRange(40, 80, intArray);
        }

        private void verifyAllValuesInRange(double low, double high, double[,] array)
		{
			for (int x = 0; x < 60; x++)
            {
				for (int z = 0; z < 50; z++)
				{
					Assert.GreaterOrEqual(high, array[x, z]);
					Assert.LessOrEqual(low, array[x, z]);
				}
            }
		}

		private void verifyAllValuesInRange(int low, int high, int[,] array)
        {
            for (int x = 0; x < 60; x++)
            {
                for (int z = 0; z < 50; z++)
                {
                    Assert.GreaterOrEqual(high, array[x, z]);
                    Assert.LessOrEqual(low, array[x, z]);
                }
            }
        }

        private void printArray<T>(T[,] array)
		{
			string output = "";
			for (int z = 0; z < 50; z++)
			{
				for (int x = 0; x < 60; x++)
				{
					output += array[x, z] + "\t";
				}
				output += "\n";
			}
			Console.Write(output);
		}
    }
}
