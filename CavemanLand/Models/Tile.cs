using CavemanLand.Utility;
using CavemanLand.Models.TileSubClasses;

namespace CavemanLand.Models
{
    public class Tile
    {
		public Coordinates coor;
		public Terrain terrain;
		public Temperatures temperatures;
		public Precipitation precipitation;
		public Rivers rivers;
		public Habitats habitats;
		public Minerals minerals;

        public Tile(int x, int z)
        {
			coor = new Coordinates(x, z);
        }
    }
}
