using System;
using System.Collections.Generic;

namespace CavemanLand.Models.TileSubClasses
{
    public class Minerals
    {
		private const double MAX_SURFACE_METAL_PERCENT = 0.25;

		private static Dictionary<string, double> metalAbundances;
		private static Random randy = new Random();

		public Dictionary<string, double> surface;
		public Dictionary<string, double> mineable;

        public Minerals(double oceanPercent, double hillPercent)
        {
			surface = new Dictionary<string, double>();
			mineable = new Dictionary<string, double>();
			double totalStone = randomStoneTotal(oceanPercent);
			double surfaceStone = splitOffSurfaceMineral(totalStone, hillPercent);
			setMinerals("Stone", surfaceStone, totalStone - surfaceStone);
			List<string> metals = getMetalNames();
            foreach(string metal in metals)
			{
				double randomNumber = randy.NextDouble();
				if (randomNumber <= getChanceOfMetal(metal))
				{
					double totalMetal = randomMetalTotal(oceanPercent, hillPercent);
					double surfaceMetal = splitOffSurfaceMineral(totalMetal * MAX_SURFACE_METAL_PERCENT, hillPercent);
					setMinerals(metal, surfaceMetal, totalMetal - surfaceMetal);
				}
   			}
        }

		public void setMinerals(string name, double surface, double mineable)
        {
			if (!this.surface.ContainsKey(name))
			{
				this.surface.Add(name, surface);
				this.mineable.Add(name, mineable);
			}
			else
			{
				this.surface[name] = surface;
				this.mineable[name] = mineable;
			}
        }

        private double splitOffSurfaceMineral(double total, double hillPercent)
		{
			return Math.Round(total * Math.Pow(hillPercent, 2.0), World.ROUND_TO);
		}

		private double randomStoneTotal(double oceanPercent)
		{
			double randomElement = 5000.0 + randy.NextDouble() * 35000.0;
			return Math.Round((1.0 - oceanPercent) * randomElement, World.ROUND_TO);
		}

        private double randomMetalTotal(double oceanPercent, double hillPercent)
		{
			return Math.Round((1000.0 + 5000.0 * randy.NextDouble() * Math.Pow(hillPercent, 2.0)) * (1.0 - oceanPercent), World.ROUND_TO);
		}

        private double getChanceOfMetal(string name)
		{
			return Math.Pow(getMetalAbundance(name), 0.5);
		}

        private double getMetalAbundance(string name)
		{
			if (metalAbundances == null)
			{
				populateMetalAbundances();
			}
			return metalAbundances[name];
		}
        
        private List<string> getMetalNames()
		{
			if (metalAbundances == null)
            {
                populateMetalAbundances();
            }
			return new List<string>(metalAbundances.Keys);
		}

		private static void populateMetalAbundances()
		{
			metalAbundances = new Dictionary<string, double>();
            // metalAbundances.Add("Aluminum", .0823);
            metalAbundances.Add("Iron", .0563);
            metalAbundances.Add("Nickel", 0.000084);
            metalAbundances.Add("Zinc", .000070);
            metalAbundances.Add("Copper", .000060);
            metalAbundances.Add("Lead", .000014);
            metalAbundances.Add("Uranium", .0000027);
            metalAbundances.Add("Coal", .0000024);
            metalAbundances.Add("Tin", .0000023);
            metalAbundances.Add("Silver", .00000075);
            metalAbundances.Add("Gold", .00000004);
		}

		private string printDictionary(string dictionaryName, Dictionary<string, double> dictionary)
        {
            string output = dictionaryName + " - \n";
            foreach (KeyValuePair<string, double> pair in dictionary)
            {
                output += pair.Key + ": " + pair.Value + "\t";
            }
            return output;
        }

		public override string ToString()
		{
			string output = printDictionary("surface minerals", surface);
			output += "\n" + printDictionary("minable minerals", mineable);
			return output;
		}

	}
}
