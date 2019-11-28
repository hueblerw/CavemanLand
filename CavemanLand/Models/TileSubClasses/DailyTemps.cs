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

		public double getAvgTemp(){
			int sum = 0;
			for (int day = 0; day < days.Length; day++){
				sum += days[day];
			}
			return (double) sum / days.Length;
		}
    }
}
