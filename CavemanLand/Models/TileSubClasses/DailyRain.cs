namespace CavemanLand.Models.TileSubClasses
{
    public class DailyRain
    {
		public int year;
		public int[] precip;
		public int[] snowCover;

        public DailyRain()
        {
        }

		public DailyRain(int year, int[] precip, int[] snowCover)
        {
			this.year = year;
			this.precip = precip;
			this.snowCover = snowCover;
        }
    }
}
