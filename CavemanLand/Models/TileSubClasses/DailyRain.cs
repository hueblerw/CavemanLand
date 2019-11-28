using CavemanLand.Utility;
using System;

namespace CavemanLand.Models.TileSubClasses
{
    public class DailyRain
    {
		public int year;
		public double[] precip;
		public double[] snowfall;
		public double[] snowCover;

        public DailyRain()
        {
        }

		public DailyRain(int year, double[] precip, double[] snowfall, double[] snowCover)
        {
			this.year = year;
			this.precip = precip;
			this.snowfall = snowfall;
			this.snowCover = snowCover;
        }

		public double getRainForYear()
        {
            double rainSum = 0.0;
            for (int day = 1; day <= WorldDate.DAYS_PER_YEAR; day++)
            {
                rainSum += precip[day - 1] + snowfall[day - 1];
            }
			return Math.Round(rainSum, World.ROUND_TO);
        }
    }
}
