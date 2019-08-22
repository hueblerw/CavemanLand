using CavemanLand.Generators;

namespace CavemanLand.Models.TileSubClasses
{
    public class Temperatures
    {
		public int low;
		public int high;
		public int summerLength;
		public double variance;
		public DailyTemps dailyTemps;

		private TemperatureEquation temperatureEquation;

		public Temperatures(int lowTemp, int highTemp, int summerLength, double variance)
        {
			this.low = lowTemp;
			this.high = highTemp;
			this.summerLength = summerLength;
			this.variance = variance;
			temperatureEquation = new TemperatureEquation(low, high, summerLength, variance);
        }
    }
}
