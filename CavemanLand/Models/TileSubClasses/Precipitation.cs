﻿using CavemanLand.Utility;

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
			int section = (int) (day - 1) / HUMIDITY_CROSS_SECTIONS;
			int remainder = (day - 1) % HUMIDITY_CROSS_SECTIONS;
			int nextSection = section + 1;
			if (section == 5){
				nextSection = 0;
			}

			return humidities[section] * ((MAX_REMAINDER - remainder) / MAX_REMAINDER) + humidities[nextSection] * (remainder / MAX_REMAINDER);
		}
    }
}
