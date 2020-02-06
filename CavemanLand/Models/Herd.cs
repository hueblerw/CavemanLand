using System.Collections.Generic;
using CavemanLand.Utility;
using CavemanLand.Models.TileSubClasses;

namespace CavemanLand.Models
{
    public class Herd
    {
		public string species;
		public List<int> popsByAge;
		public List<Coordinates> range; // list with a variable maximum size?
		public double fatReserve;
		public List<TileMemory> memories;

		private double foodConsumption;
		private int LOS;
		private int speed;
		private int spread;

        public Herd()
        {
        }

        public void consumeFood()
		{
			return;
		}

        public void migrate()
		{
			return;
		}

        public bool determineIfMigrate()
		{
			return false;
		}
    }
}
