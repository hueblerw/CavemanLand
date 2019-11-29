using System.Collections.Generic;
using CavemanLand.Utility;
using Newtonsoft.Json;
using System;

namespace CavemanLand.Models.TileSubClasses
{
    public class Habitats
    {
		public const int NUMBER_OF_HABITATS = 14;
		private const int REGROWTH_MULTIPLIER = 100 / World.YEARS_TO_FULL_HABITAT_REGROWTH;
		private const int REPLACEMENT_MULTIPLIER = 1;
		private const int QUALITY_MULTIPLIER = 2;
		private const double RIVER_WATERING_MULTIPLIER = 0.1;
		private const double QUALITY_CHANGE_MULTIPLIER = 0.1;
		private const double QUALITY_MIN = 0.0;
		private const double QUALITY_MAX = 10.0;
        
		public int[] typePercents;
		public double[] currentLevel;
		public double[] gameCurrentLevel;

		private static Dictionary<int, string> habitatMapping;
		private static Dictionary<string, Dictionary<string, double>> ideals;
		private int percentEmpty;
		private Random randy;
        
		public Habitats(double oceanPercent)
        {
			string idealsString = loadDataFileToString("Ideals.json");
			if (ideals == null){
				ideals = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(idealsString);
				habitatMapping = createMapping();
			}
			randy = new Random();
			this.typePercents = new int[NUMBER_OF_HABITATS];
			this.currentLevel = generateRandomLevels();
			this.gameCurrentLevel = currentLevel;
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

		public void growHabitats(double avgTemp, double totalPrecipitation, double avgRiverLevel, bool isOcean, bool isIceSheet){
			if (!isOcean)
			{
				calculatePercentEmpty();
				int growthAmount = Math.Max(Math.Min(percentEmpty, REGROWTH_MULTIPLIER), REPLACEMENT_MULTIPLIER);
				int favoredHabitatIndex = determineFavoredHabitat(avgTemp, totalPrecipitation, avgRiverLevel, isIceSheet);
				if (percentEmpty > 0)
				{
					growthAmount = Math.Min(growthAmount, percentEmpty);
					typePercents[favoredHabitatIndex] = Math.Min(typePercents[favoredHabitatIndex] + growthAmount, 100);
				}
				else
				{
					int randomHabitatIndex = generateRandomActiveHabitatIndex();
					if (randomHabitatIndex != favoredHabitatIndex)
					{
						// habitat expansion
						typePercents[randomHabitatIndex] = Math.Max(typePercents[randomHabitatIndex] - growthAmount, 0);
						typePercents[favoredHabitatIndex] = Math.Min(typePercents[favoredHabitatIndex] + growthAmount, 100);
						// quality reduced in lost habitat
						currentLevel[randomHabitatIndex] = Math.Round(Math.Max(currentLevel[randomHabitatIndex] - growthAmount * QUALITY_CHANGE_MULTIPLIER, QUALITY_MIN), World.ROUND_TO);
						if (currentLevel[randomHabitatIndex] < gameCurrentLevel[randomHabitatIndex])
						{
							gameCurrentLevel[randomHabitatIndex] = Math.Round(Math.Max(growthAmount * QUALITY_CHANGE_MULTIPLIER, QUALITY_MIN), World.ROUND_TO);
						}
					}
				}
				// quality increased
                double change = getQualityChange(favoredHabitatIndex, avgTemp, totalPrecipitation, avgRiverLevel, isIceSheet);
                double newLevel = Math.Round(currentLevel[favoredHabitatIndex] + change, World.ROUND_TO);
                double newGameLevel = Math.Round(gameCurrentLevel[favoredHabitatIndex] + change, World.ROUND_TO);
                currentLevel[favoredHabitatIndex] = Math.Min(Math.Max(QUALITY_MIN, newLevel), QUALITY_MAX);
				gameCurrentLevel[favoredHabitatIndex] = Math.Min(Math.Max(QUALITY_MIN, newGameLevel), QUALITY_MAX);
			}
		}

		private static string loadDataFileToString(string pathname)
        {
            return MyJsonFileInteractor.loadJsonFileToString(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/DataFiles/" + pathname);
        }

		private double[] generateRandomLevels(){
			double[] array = new double[NUMBER_OF_HABITATS];
			for (int i = 0; i < NUMBER_OF_HABITATS; i++){
				array[i] = generateRandomLevel();
			}
			return array;
		}

		private double generateRandomLevel()
		{
			return Math.Round(randy.NextDouble() * 10.0, World.ROUND_TO);
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
				double functionalPrecipitation = getFunctionalPrecip(avgRiverLevel, totalPrecipitation);
				int multiplier = getFavoredTempRegion(avgTemp);
				int addifier = getFavoredPrecipLevel(functionalPrecipitation);
				return 4 * multiplier + addifier;
			}
		}

		private double getQualityChange(int habitatIndex, double avgTemp, double totalPrecipitation, double avgRiverLevel, bool isIceSheet)
        {
            if (isIceSheet)
            {
                // Ice Sheet Index
                return REPLACEMENT_MULTIPLIER * QUALITY_MULTIPLIER;
            }
            else
            {
				// Get data
				string habitatName = habitatMapping[habitatIndex];
				double functionalPrecip = getFunctionalPrecip(avgRiverLevel, totalPrecipitation);
				double idealTemp = ideals[habitatName]["avgTemp"];
				double tempPlusMinus = ideals[habitatName]["tempPlusMinus"];
				double idealRain = ideals[habitatName]["rain"];
				double rainPlusMinus = ideals[habitatName]["tempPlusMinus"];
                // calculate the change in quality.
				double tempChange = 1.5 * QUALITY_MULTIPLIER * Math.Abs(avgTemp - idealTemp) / tempPlusMinus;
				double rainChange = 1.5 * QUALITY_MULTIPLIER * Math.Abs(functionalPrecip - idealRain) / rainPlusMinus;
				return Math.Max(0.0, (tempChange + rainChange) / 2.0);
            }
        }

		private int getFavoredTempRegion(double avgTemp){
			// Artic
			int multiplier = 0;
			switch (avgTemp)
            {
				case double temp when (temp > 45.0 && temp < 70):
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

		private double getFunctionalPrecip(double avgRiverLevel, double totalPrecipitation){
			return avgRiverLevel * RIVER_WATERING_MULTIPLIER + totalPrecipitation;
		}

    }
}
