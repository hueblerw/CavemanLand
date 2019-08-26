namespace CavemanLand.Models.TileSubClasses
{
    public class DailyTemps
    {
		public int year;
		public int[] days;

        public DailyTemps()
        {
        }

		public DailyTemps(int year, int[] days)
		{
			this.year = year;
			this.days = days;
		}
    }
}
