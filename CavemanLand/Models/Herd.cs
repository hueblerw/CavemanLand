using System.Collections.Generic;
using CavemanLand.Utility;
using CavemanLand.Models.TileSubClasses;

namespace CavemanLand.Models
{
    public class Herd
    {
		public string species;
		public List<int> popsByAge;
		public Coordinates location;
		public double fatReserve;
		public List<TileMemory> memories;
        
        public Herd()
        {
        }
    }
}
