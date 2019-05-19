namespace CavemanLand.Models.TileSubClasses
{
    public class Terrain
    {
		public double elevation;
		public double oceanPercent;
		public double hillPercent;

        public Terrain()
        {
        }

		public Terrain(double elevation, double oceanPercent, double hillPercent)
        {
			this.elevation = elevation;
			this.oceanPercent = oceanPercent;
			this.hillPercent = hillPercent;
        }
    }
}
