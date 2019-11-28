using CavemanLand.Utility;
using System;

namespace CavemanLand.Models.TileSubClasses
{
    public class DailyVolume
    {
		public int year;
		public double[] volume;

		public DailyVolume()
		{
		}

        public DailyVolume(int year, double[] volume)
        {
			this.year = year;
			this.volume = volume;
        }

        public double getAverageWaterLevel()
		{
			double sum = 0.0;
			for (int day = 1; day <= WorldDate.DAYS_PER_YEAR; day++){
				sum += volume[day - 1];
			}
			return Math.Round(sum / WorldDate.DAYS_PER_YEAR, 2);
		}
    }
}
