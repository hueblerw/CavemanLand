using System.Collections.Generic;
using System;

namespace CavemanLand.Models.TileSubClasses
{
    public class Habitats
    {
		public const int NUMBER_OF_HABITATS = 14;
		private const int REGROWTH_MULTIPLIER = 100 / World.YEARS_TO_FULL_HABITAT_REGROWTH;
		private const int REPLACEMENT_MULTIPLIER = 1;
		private const double RIVER_WATERING_MULTIPLIER = 0.1; 
        
		public int[] typePercents;
		public double[] currentLevel;
		public double[] gameCurrentLevel;

		private Dictionary<int, string> habitatMapping;
		private int percentEmpty;
		private Random randy;
        
		public Habitats(double oceanPercent)
        {
			this.typePercents = new int[NUMBER_OF_HABITATS];
			habitatMapping = createMapping();
			this.currentLevel = new double[NUMBER_OF_HABITATS];
			this.gameCurrentLevel = currentLevel;
			randy = new Random();
			// set oceanPercent
			this.typePercents[13] = (int) (oceanPercent * 100);
        }

		public int calculatePercentEmpty(){
			percentEmpty = 100;
			for (int i = 0; i < NUMBER_OF_HABITATS; i++){
				percentEmpty -= typePercents[i];
			}
			return percentEmpty;
		}

		public void growHabitats(double avgTemp, double totalPrecipitation, double avgRiverLevel, bool isOcean, bool isIceSheeet){
			if (!isOcean)
			{
				calculatePercentEmpty();
				int growthAmount = Math.Max(Math.Min(percentEmpty, REGROWTH_MULTIPLIER), REPLACEMENT_MULTIPLIER);
				int favoredHabitatIndex = determineFavoredHabitat(avgTemp, totalPrecipitation, avgRiverLevel, isIceSheeet);
				if (percentEmpty > 0)
				{
					growthAmount = Math.Min(growthAmount, percentEmpty);
					typePercents[favoredHabitatIndex] += growthAmount;
				}
				else
				{
					int randomHabitatIndex = generateRandomActiveHabitatIndex();
					typePercents[randomHabitatIndex] -= growthAmount;
					typePercents[favoredHabitatIndex] += growthAmount;
				}
			}
		}

		private double generateRandomLevel()
		{
			return randy.NextDouble() * 10.0;
		}

		private int generateRandomActiveHabitatIndex(){
			List<int> activeIndexes = getActiveHabitatIndexes();
			int randomIndex = randy.Next(activeIndexes.Count);
			return activeIndexes[randomIndex];
		}

		private List<int> getActiveHabitatIndexes(){
			List<int> activeIndexes = new List<int>();
            // -1 ensures we never overwrite the oceans.
            for (int i = 0; i < typePercents.Length - 1; i++)
            {
                if (typePercents[i] > 0)
                {
                    activeIndexes.Add(i);
                }
            }

			return activeIndexes;
		}

		private Dictionary<int, string> createMapping()
		{
			Dictionary<int, string> mapping = new Dictionary<int, string>();
			mapping.Add(0, "Artic Desert");
			mapping.Add(1, "Tundra");
			mapping.Add(2, "Boreal");
			mapping.Add(3, "Artic Marsh");
			mapping.Add(4, "Desert");
			mapping.Add(5, "Plains");
			mapping.Add(6, "Forest");
			mapping.Add(7, "Swamp");
			mapping.Add(8, "Hot Desert");
			mapping.Add(9, "Savannah");
			mapping.Add(10, "Monsoon Forest");
			mapping.Add(11, "Rainforest");
			mapping.Add(12, "Ice Sheet");
			mapping.Add(13, "Ocean");

			return mapping;
		}
        
		private int determineFavoredHabitat(double avgTemp, double totalPrecipitation, double avgRiverLevel, bool isIceSheeet){
            if(isIceSheeet){
				// Ice Sheet Index
				return 12;
			} else {
				double functionalPrecipitation = avgRiverLevel * RIVER_WATERING_MULTIPLIER + totalPrecipitation;
				int multiplier = getFavoredTempRegion(avgTemp);
				int addifier = getFavoredPrecipLevel(functionalPrecipitation);
				return 4 * multiplier + addifier;
			}
		}

		private int getFavoredTempRegion(double avgTemp){
			// Artic
			int multiplier = 0;
			switch (avgTemp)
            {
				case double temp when (temp > 40.0 && temp < 70):
                    // Temperate
                    multiplier = 1;
                    break;
                case double temp when (temp >= 70.0):
					// Tropical
                    multiplier = 2;
                    break;
            }

			return multiplier;
		}

		private int getFavoredPrecipLevel(double functionalPrecipitation){
			// Very Dry
            int addifier = 0;
            switch (functionalPrecipitation)
            {
				case double wet when (wet >= 20.0 && wet < 40.0):
                    // Dry
					addifier = 1;
                    break;
				case double wet when (wet >= 40.0 && wet < 70.0):
                    // Moderate
					addifier = 2;
                    break;
				case double wet when (wet >= 70.0):
                    // Wet
					addifier = 3;
                    break;
            }

			return addifier;
		}

    }
}
