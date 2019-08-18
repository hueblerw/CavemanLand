using NUnit.Framework;
using CavemanLand.Generators;
using System;
using System.Collections.Generic;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class TemperatureEquationTestClass
    {
        [Test()]
		public void TemperatureInGeneralRangeTestCase()
        {
			TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 60, 6.0);
			for (int day = 1; day <= 120; day++){
				int temp = temperatureEquation.getTodaysTemp(day);
				assertBetween(day, temp, 20 - 6 / 2, 80 + 6 / 2);            
			}
        }

		[Test()]
        public void MonotonicTestCase()
        {
			TemperatureEquation[] temperatureEquations = new TemperatureEquation[3];
			temperatureEquations[0] = new TemperatureEquation(20, 80, 40, 0.0);
            temperatureEquations[1] = new TemperatureEquation(20, 80, 60, 0.0);
			temperatureEquations[2] = new TemperatureEquation(20, 80, 80, 0.0);
			Console.WriteLine("Monotonic at various points check:");
			for (int i = 0; i < 3; i++){
				printYearOfTemps(temperatureEquations[i]);
				verifyIsMonotonic(temperatureEquations[i], (i + 1) * 20 + 20);
			}
        }
        
		[Test()]
        public void NormalBoundariesTemperaturesCorrectTestCase()
        {
            TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 60, 6.0);
			Console.WriteLine("Normal Boundaries");
			printYearOfTemps(temperatureEquation);
            assertBetween(1, temperatureEquation.getTodaysTemp(1), 20 - 6 / 2, 20 + 6 / 2 + 1);
            assertBetween(120, temperatureEquation.getTodaysTemp(120), 20 - 6 / 2, 20 + 6 / 2);
            assertBetween(61, temperatureEquation.getTodaysTemp(61), 80 - 6 / 2, 80 + 6 / 2 + 1);
			assertBetween(91, temperatureEquation.getTodaysTemp(91), 50 - 6 / 2, 50 + 6 / 2 + 1);
            assertBetween(31, temperatureEquation.getTodaysTemp(31), 50 - 6 / 2, 50 + 6 / 2 + 1);
        }

		[Test()]
        public void ShortSummerTemperaturesCorrectTestCase()
        {
            TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 30, 6.0);
			Console.WriteLine("Short Summer");
			printYearOfTemps(temperatureEquation);
            assertBetween(1, temperatureEquation.getTodaysTemp(1), 20 - 6 / 2, 20 + 6 / 2 + 1);
            assertBetween(120, temperatureEquation.getTodaysTemp(120), 20 - 6 / 2, 20 + 6 / 2);
            assertBetween(61, temperatureEquation.getTodaysTemp(61), 80 - 6 / 2, 80 + 6 / 2 + 1);
            assertBetween(91, temperatureEquation.getTodaysTemp(76), 50 - 6 / 2, 50 + 6 / 2 + 1);
            assertBetween(31, temperatureEquation.getTodaysTemp(46), 50 - 6 / 2, 50 + 6 / 2 + 1);
        }
        
		[Test()]
        public void LongSummerTemperaturesCorrectTestCase()
        {
            TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 90, 6.0);
			Console.WriteLine("Long Summer");
			printYearOfTemps(temperatureEquation);
            assertBetween(1, temperatureEquation.getTodaysTemp(1), 20 - 6 / 2, 20 + 6 / 2 + 1);
            assertBetween(120, temperatureEquation.getTodaysTemp(120), 20 - 6 / 2, 20 + 6 / 2);
            assertBetween(61, temperatureEquation.getTodaysTemp(61), 80 - 6 / 2, 80 + 6 / 2 + 1);
            assertBetween(91, temperatureEquation.getTodaysTemp(106), 50 - 6 / 2, 50 + 6 / 2 + 1);
            assertBetween(31, temperatureEquation.getTodaysTemp(16), 50 - 6 / 2, 50 + 6 / 2 + 1);
        }

		[Test()]
        public void LowVarianceTemperaturesCorrectTestCase()
        {
            TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 60, 2.0);
			Console.WriteLine("Low Variance");
			printYearOfTemps(temperatureEquation);
            assertBetween(1, temperatureEquation.getTodaysTemp(1), 20 - 2 / 2, 20 + 2 / 2 + 1);
            assertBetween(120, temperatureEquation.getTodaysTemp(120), 20 - 2 / 2, 20 + 2 / 2);
            assertBetween(60, temperatureEquation.getTodaysTemp(60), 80 - 2 / 2, 80 + 2 / 2 + 1);
            assertBetween(90, temperatureEquation.getTodaysTemp(90), 50 - 2 / 2, 50 + 2 / 2 + 1);
            assertBetween(30, temperatureEquation.getTodaysTemp(30), 50 - 2 / 2, 50 + 2 / 2 + 1);
        }

		[Test()]
        public void HighVarianceTemperaturesCorrectTestCase()
        {
            TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 60, 12.0);
			Console.WriteLine("High Variance");
			printYearOfTemps(temperatureEquation);
            assertBetween(1, temperatureEquation.getTodaysTemp(1), 20 - 12 / 2, 20 + 12 / 2 + 1);
            assertBetween(120, temperatureEquation.getTodaysTemp(120), 20 - 12 / 2, 20 + 12 / 2);
            assertBetween(61, temperatureEquation.getTodaysTemp(60), 80 - 12 / 2, 80 + 12 / 2 + 1);
            assertBetween(91, temperatureEquation.getTodaysTemp(90), 50 - 12 / 2, 50 + 12 / 2 + 1);
            assertBetween(31, temperatureEquation.getTodaysTemp(30), 50 - 12 / 2, 50 + 12 / 2 + 1);
        }      

        private void assertBetween(int day, int value, int lowerBound, int higherBound)
		{
			Assert.LessOrEqual(value, higherBound, "Value [" + value + "] is above higherBound [" + higherBound + "] for day: " + day);
			Assert.GreaterOrEqual(value, lowerBound, "Value [" + value + "] is below lowerBound [" + lowerBound + "] for day: " + day);
		}

        // The new function is only monotonic for 35.2 < summerLength < 84.8
		private void verifyIsMonotonic(TemperatureEquation temperatureEquation, int summerLength)
		{
			double lastTemp = -1000;
			double temp;
			List<int> failures = new List<int>();
			for (int day = 1; day <= 60; day++)
			{
				temp = temperatureEquation.getTodaysTemp(day);
                if(temp < lastTemp)
				{
					failures.Add(day);
				}
				lastTemp = temp;
			}
			for (int day = 61; day <= 120; day++)
            {
                temp = temperatureEquation.getTodaysTemp(day);
                if (temp > lastTemp)
                {
                    failures.Add(day);
                }
                lastTemp = temp;
            }
			Assert.AreEqual(0, failures.Count, "For summer of length " + summerLength + " there were " + failures.Count + " entries that were not monotonic: " + printList(failures));
		}

        private void printYearOfTemps(TemperatureEquation temperatureEquation)
		{
			for (int day = 1; day <= 120; day++)
            {
                int temp = temperatureEquation.getTodaysTemp(day);
                Console.Write(temp + ", ");
            }
			Console.Write("\n");
		}

        private string printList(List<int> list)
		{
			string output = "";
			int[] array = list.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				output += array[i];
				if (i != array.Length - 1){
					output += ", ";
				}
			}
			return output;
		}
    }
}
