using CavemanLand.Models.GenericModels;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CavemanLand.Models
{
    public class World
    {
		public static Animal[] animalSpecies;
		public static Plant[] plantSpecies;

		private int x;
		private int z;
		private Tile[,] tileArray;
		private List<Herd> herds;
		private List<Tribe> tribes;

        public World(int x, int z)
        {
			this.x = x;
			this.z = z;
			tileArray = generateTileArray();
        }

        public string tileArrayToJson()
		{
			return JsonConvert.SerializeObject(tileArray);
		}

        private Tile[,] generateTileArray()
		{
			return null;
		}
    }
}
