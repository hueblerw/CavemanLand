using System;

namespace CavemanLand.Utility
{
    public class WorldDate
    {
		public const int DAYS_PER_YEAR = 120;

		public int year;
		public int day;

        public WorldDate(int year, int day)
        {
			this.year = year;
			this.day = day;
        }

		public override string ToString()
		{
			return "year: " + year + "\tday: " + day;
		}
	}
}
