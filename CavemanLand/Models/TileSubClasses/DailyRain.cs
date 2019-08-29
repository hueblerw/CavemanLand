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
    }
}
