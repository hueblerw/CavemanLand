using System.Collections.Generic;

namespace CavemanLand
{
    public class Plant
    {
		private string name;
		private string type;
		private bool isFarmable;
		private List<string> habitats;
		private List<int> abundance;
		private int[] temperatureRange = new int[2];
		private double[] rainfallRange = new double[2];
		private int growthPeriod;
		private bool doesFrostKill;
		private string resourceProduced;
		private double unitPerHarvest;
		private double weightPerUnit;

        public Plant()
        {
        }
    }
}
