using CavemanLand.Utility;

namespace CavemanLand.Models.TileSubClasses
{
    public class TileMemory
    {
		public Coordinates coordinates;
		public double value;

        public TileMemory()
        {
        }

		public TileMemory(Coordinates coordinates, double value)
        {
			this.coordinates = coordinates;
			this.value = value;
        }
    }
}
