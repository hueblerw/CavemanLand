using System.Collections.Generic;
using CavemanLand.Utility;

namespace CavemanLand.Models
{
	public class Tribe : Herd
    {
		public List<int> pops;
		public Coordinates location;
		public List<Coordinates> rememberedTiles;

        public Tribe()
        {
        }
    }
}
