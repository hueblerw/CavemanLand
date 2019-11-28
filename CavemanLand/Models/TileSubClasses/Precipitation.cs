using CavemanLand.Utility;
using System;

namespace CavemanLand.Models.TileSubClasses
{
    public class Precipitation
    {
		public const int HUMIDITY_CROSS_SECTIONS = 6;
		private const int MAX_REMAINDER = WorldDate.DAYS_PER_YEAR / 6;
		public double[] humidities;
		public DailyRain dailyRain;

        public Precipitation(double[] humidities)
        {
			this.humidities = humidities;
        }

        public double getHumidity(int day)
		{
			int section = (int) (day - 1) / MAX_REMAINDER;
			int remainder = (day - 1) % MAX_REMAINDER;
			int nextSection = section + 1;
			if (section == 5){
				nextSection = 0;
			}

			double maxRemainder = (double) MAX_REMAINDER;
			return Math.Round(humidities[section] * ((MAX_REMAINDER - remainder) / maxRemainder) + humidities[nextSection] * (remainder / maxRemainder), World.ROUND_TO);
		}

        public void setDailyRain(DailyRain dailyRain)
		{
			this.dailyRain = dailyRain;
		}

        public double getRainForYear()
		{
			return dailyRain.getRainForYear();
		}
    }
}
