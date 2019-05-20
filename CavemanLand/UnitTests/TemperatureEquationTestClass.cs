using NUnit.Framework;
using CavemanLand.Generators;
using System;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class TemperatureEquationTestClass
    {
        [Test()]
		public void TemperatureInGeneralRangeTestCase()
        {
			TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 60, 6.0);
			Console.WriteLine(temperatureEquation.ToString());
			for (int day = 1; day <= 120; day++){
				int temp = temperatureEquation.getTodaysTemp(day);
				Console.Write(temp + ", ");
				assertBetween(temp, 20 - 6 / 2, 80 + 6 / 2);            
			}
        }
        
		[Test()]
        public void BoundariesTemperaturesCorrectTestCase()
        {
            TemperatureEquation temperatureEquation = new TemperatureEquation(20, 80, 60, 6.0);         
            assertBetween(temperatureEquation.getTodaysTemp(1), 20 - 6 / 2, 20 + 6 / 2);
            assertBetween(temperatureEquation.getTodaysTemp(120), 20 - 6, 20 + 6 / 2);
            assertBetween(temperatureEquation.getTodaysTemp(61), 80 - 6 / 2, 80 + 6 / 2);
			assertBetween(temperatureEquation.getTodaysTemp(91), 50 - 6 / 2, 50 + 6 / 2);
            assertBetween(temperatureEquation.getTodaysTemp(31), 50 - 6 / 2, 50 + 6 / 2);
        }

        private void assertBetween(int value, int lowerBound, int higherBound)
		{
			Assert.LessOrEqual(value, higherBound);
            Assert.GreaterOrEqual(value, lowerBound);
		}
    }
}
