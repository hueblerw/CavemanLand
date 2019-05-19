using System.Collections.Generic;

namespace CavemanLand.Models
{
    public class Plant
    {
		public string name;
		public string type;
		public bool isFarmable;
		public List<string> habitats;
		public List<int> abundance;
		public int[] temperatureRange = new int[2];
		public double[] rainfallRange = new double[2];
		public int growthPeriod;
		public bool doesFrostKill;
		public string resourceProduced;
		public double unitPerHarvest;
		public double weightPerUnit;

        public Plant()
        {
        }

		public override string ToString(){
			return name;
		}

    }
}
