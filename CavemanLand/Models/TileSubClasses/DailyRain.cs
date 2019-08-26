namespace CavemanLand.Models.TileSubClasses
{
    public class DailyRain
    {
		public int year;
		public int[] precip;
		public int[] snowfall;
		public int[] snowCover;

        public DailyRain()
        {
        }

		public DailyRain(int year, int[] precip, int[] snowfall, int[] snowCover)
        {
			this.year = year;
			this.precip = precip;
			this.snowfall = snowfall;
			this.snowCover = snowCover;
        }
    }
}
