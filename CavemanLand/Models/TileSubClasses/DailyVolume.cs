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
    }
}
