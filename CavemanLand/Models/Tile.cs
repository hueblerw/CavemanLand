using System;
using System.Collections.Generic;
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

		public string getTileInfo(int day){
			string output = coor.ToString();
			output += "\n" + terrain;
			output += "\nTemperature: " + temperatures.dailyTemps.days[day];
			output += "\nRainfall: " + precipitation.dailyRain.precip[day];
			output += "\nSurface Water: " + rivers.dailyVolume.volume[day];
			output += "\n" + habitats;
			output += "\n" + printDictionary("Vegetation", getVegetation(day));
			output += "\n" + printDictionary("Gatherables", getTodaysGatherables(day));
			output += "\n" + minerals;
			return output;
		}
        
        public Dictionary<string, double> getVegetation(int day)
		{
			Dictionary<string, double> vegetation = new Dictionary<string, double>();
			int todaysTemp = temperatures.dailyTemps.days[day];
            double grass;
            double last5Rain = 0.0;
            int firstDay = Math.Max(0, day - 4);
            for (int d = firstDay; d <= day; d++)
            {
                last5Rain += precipitation.dailyRain.precip[d] + rivers.dailyVolume.volume[d] * Habitats.RIVER_WATERING_MULTIPLIER;
            }
            // Grazing
            double grazing = habitats.getGrazing(last5Rain, todaysTemp, out grass);
			vegetation.Add("grazing", grazing);
            // Trees
			double trees = habitats.getTrees();
			vegetation.Add("trees", trees);
			// Seeds
			double seeds = habitats.getSeeds(grass, (int) trees);
			vegetation.Add("seeds", seeds);
			// Foilage
			double foilage = habitats.getFoilage((int) trees, todaysTemp);
			vegetation.Add("foilage", foilage);
            
			return vegetation;
		}

        public Dictionary<string, double> getTodaysGatherables(int day)
		{
			Dictionary<string, double[]> yearOfGatherables = habitats.getYearOfGatherables(temperatures.dailyTemps, precipitation.dailyRain, rivers.dailyVolume);
			Dictionary<string, double> todaysGatherables = new Dictionary<string, double>();
            foreach(KeyValuePair<string, double[]> pair in yearOfGatherables)
			{
				todaysGatherables.Add(pair.Key, pair.Value[day]);
			}

			return todaysGatherables;
		}

        private string printDictionary(string dictionaryName, Dictionary<string, double> dictionary)
		{
			string output = dictionaryName + " - \n";
			foreach(KeyValuePair<string, double> pair in dictionary)
			{
				output += pair.Key + ": " + pair.Value + "\t";
			}
			return output;
		}
        
    }
}
