using CavemanLand.Generators;
using CavemanLand.Utility;

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
        
		public void generateYearsTemps(int year){
			int[] temps = new int[WorldDate.DAYS_PER_YEAR];
			for (int day = 1; day <= temps.Length; day++){
				temps[day - 1] = temperatureEquation.getTodaysTemp(day);
			}
			dailyTemps = new DailyTemps(year, temps);
		}
    }
}
