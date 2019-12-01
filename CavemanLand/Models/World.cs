using CavemanLand.Models.GenericModels;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using CavemanLand.Utility;
using CavemanLand.Generators;
using CavemanLand.Models.TileSubClasses;

namespace CavemanLand.Models
{
    public class World
    {
        // World Generation Constants
		public const int YEARS_TO_FULL_HABITAT_REGROWTH = 20;
		private const double UNLIMITED_MIN = -1000.0;
		private const double UNLIMITED_MAX = -UNLIMITED_MIN;
            // Elevation
    		private const double STARTING_ELE_RANGE = 10.0;
            private const double STARTING_ELE_MIN = -5.0;
            private const double ELE_CHANGE_BY = 2.0;
		    // Temperature 
		    private const int LOW_TEMP_MIN = -25;
		    private const int LOW_TEMP_MAX = 75;
    		private const int HIGH_TEMP_MIN = 15;
            private const int HIGH_TEMP_MAX = 115;
		    private const int TEMP_CHANGE_BY = 2;
    		private const double STARTING_HIGH_TEMP_RANGE = 35.0;
		    private const double STARTING_HIGH_TEMP_MIN = 50.0;
    		private const double STARTING_LOW_TEMP_RANGE = 30.0;
            private const double STARTING_LOW_TEMP_MIN = 5.0;
    		private const int SUMMER_LENGTH_MIN = 36;
    		private const int SUMMER_LENGTH_MAX = 84;
            private const double VARIANCE_MIN = 0.0;
    		private const double VARIANCE_MAX = 12.0;
            private const double VARIANCE_CHANGE_BY = 1.0;
            private const double STARTING_SUMMER_LENGTH_RANGE = 20.0;
    		private const double STARTING_SUMMER_LENGTH_MIN = 40.0;
    		// Precipitation
    		private const double HUMIDITY_MIN = 0.0;
    		private const double HUMIDITY_MAX = 12.0;
    		private const double HUMIDITY_CHANGE_BY = 2.0;
    		private const double RAINFALL_MULTIPLIER = 3.0;
            private const int RAINFALL_POWER = 2;
    		// Rivers
    		private const double FLOW_RATE_MULT = 0.2;
		    private const double MELT_CONST = ((3.0 * 1.8) / 25.4);
            
        // General Constants
		public const int ROUND_TO = 2;
		private const double FREEZING_POINT = 32.0;
		private const string SAVE_FILE_LOCATION = @"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/SaveFiles/";

        // Variables
		public static Animal[] animalSpecies;
		public static Plant[] plantSpecies;
		public WorldDate currentDate;

		private string worldName = "WorldA";
		private WorldFile worldFile;
		private int x;
		private int z;
		private Tile[,] tileArray;
		private List<Herd> herds;
		private List<Tribe> tribes;
		private double maxDiff;
        
		private LayerGenerator doubleLayerGenerator;
		private LayerGenerator intLayerGenerator;
		private Random randy;

		// This constructor is for generating new worlds from scratch with a custom name
        public World(int x, int z, string worldName)
        {
            this.worldName = worldName;
            buildWorld(x, z);
        }

        // This constructor is for generating new worlds from scratch without a name
        public World(int x, int z)
        {
            buildWorld(x, z);
        }

		private void buildWorld(int x, int z){
			this.x = x;
            this.z = z;
            Coordinates.setWorldSize(x, z);
            currentDate = new WorldDate(1, 1);
            herds = new List<Herd>();
            tribes = new List<Tribe>();
            doubleLayerGenerator = new LayerGenerator(x, z, ROUND_TO);
            intLayerGenerator = new LayerGenerator(x, z, 0);
            randy = new Random();
            maxDiff = 0.0;
            tileArray = generateTileArray();
            // Once the basic stats are generated - generate 20 years of weather
            // This finishes the rivers, and gives the data to generate the habitats.
            for (int year = 0; year < YEARS_TO_FULL_HABITAT_REGROWTH; year++)
            {
                generateYearOfWeather(year + 1);
            }
		}

        // This constructor is intended only for loading extant worlds from File
		private World(WorldFile worldFile, Tile[,] tileArray, List<Herd> herds, List<Tribe> tribes)
		{
			this.x = worldFile.dimensions[0];
            this.z = worldFile.dimensions[1];
            Coordinates.setWorldSize(x, z);
            currentDate = new WorldDate(1, 1);
            this.herds = herds;
            this.tribes = tribes;
            doubleLayerGenerator = new LayerGenerator(x, z, ROUND_TO);
            intLayerGenerator = new LayerGenerator(x, z, 0);
            randy = new Random();
			this.tileArray = tileArray;
		}
        
        public string tileArrayToJson()
		{
			return JsonConvert.SerializeObject(tileArray);
		}

		public void saveGameFiles(string worldName)
		{
			this.worldName = worldName;
			int[] dim = { x, z };
			string tileFileLocation = concatenateFileName("tiles");
			string herdFileLocation = concatenateFileName("herds");
			string tribeFileLocation = concatenateFileName("tribes");
			worldFile = new WorldFile(worldName, dim, currentDate, tileFileLocation, herdFileLocation, tribeFileLocation);
            // serialize jsons for worldFile, tileArray, herds, and tribes
			string worldFileJson = JsonConvert.SerializeObject(worldFile);
			string worldArrayFileJson = JsonConvert.SerializeObject(tileArray);
			string herdsFileJson = JsonConvert.SerializeObject(herds);
			string tribesFileJson = JsonConvert.SerializeObject(tribes);
			// Then create / save the json files.
			writeFileToPath("worldFile", worldFileJson);
			writeFileToPath("tiles", worldArrayFileJson);
			writeFileToPath("herds", herdsFileJson);
			writeFileToPath("tribes", tribesFileJson);
		}
        
        public static World loadGameFiles(string worldName)
		{
			string json = loadGameFileToString(worldName + "-worldFile.json");
			WorldFile worldFile = JsonConvert.DeserializeObject<WorldFile>(json);
			json = loadGameFileToString(worldName + "-tiles.json");
            Tile[,] tileArray = JsonConvert.DeserializeObject<Tile[,]>(json);
			json = loadGameFileToString(worldName + "-herds.json");
			List<Herd> herds = JsonConvert.DeserializeObject<List<Herd>>(json);
			json = loadGameFileToString(worldName + "-tribes.json");
			List<Tribe> tribes = JsonConvert.DeserializeObject<List<Tribe>>(json);
			return new World(worldFile, tileArray, herds, tribes);
		}

		public void generateNewYear()
		{
			currentDate.year++;
			generateYearOfWeather(currentDate.year);
		}

		public Tile[,] getTileArray()
		{
			return tileArray;
		}

		private static string loadGameFileToString(string pathname)
        {
            return MyJsonFileInteractor.loadJsonFileToString(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/SaveFiles/" + pathname);
        }

        private void writeFileToPath(string filename, string json)
		{
			MyJsonFileInteractor.writeFileToPath(concatenateFileName(filename), json);
		}

        private string concatenateFileName(string filename)
		{
			return SAVE_FILE_LOCATION + worldName + "-" + filename + ".json";
		}

		private Tile[,] generateTileArray()
		{
			Tile[,] tiles = new Tile[this.x, this.z];
			// Generate Objects
			double[,] elevations = generateElevationMap();
			maxDiff = calculateMaxDiff(elevations);
			int[,] lowTemps = generateLowTemps();
			int[,] highTemps = generateHighTemps(lowTemps);
			int[,] summerLengths = generateSummerLength();
			double[,] variances = generateVariance();
			double[][,] humidities = generateHumidities();
			double[,] flowRates;
			Direction.CardinalDirections[,] downstreamDirections = calculateDownStreams(elevations, out flowRates);
            // Generate Mineral Layers

			// Populate layers
			for (int x = 0; x < this.x; x++)
			{
				for (int z = 0; z < this.z; z++)
				{
					tiles[x, z] = new Tile(x, z);
					double oceanPer = calculateOceanPercentage(tiles[x, z].coor, elevations);
					tiles[x, z].terrain = new Terrain(elevations[x, z], oceanPer, calculateHillPercentage(tiles[x, z].coor, elevations, oceanPer));
					tiles[x, z].temperatures = new Temperatures(lowTemps[x, z], highTemps[x, z], summerLengths[x, z], variances[x, z]);
					tiles[x, z].precipitation = new Precipitation(convertThisTilesHumiditiesToArray(x, z, humidities));
					List<Direction.CardinalDirections> upstreamDirections = getUpstreamFromDownstream(x, z, downstreamDirections);
					tiles[x, z].rivers = new Rivers(downstreamDirections[x, z], upstreamDirections, flowRates[x, z]);
					tiles[x, z].habitats = new Habitats(oceanPer);
                    // Then Minerals
				}
			}
            
			return tiles;
		}
        
		private double[,] generateElevationMap()
		{
			double startingValue = randy.NextDouble() * STARTING_ELE_RANGE + STARTING_ELE_MIN;
			return doubleLayerGenerator.GenerateWorldLayer(UNLIMITED_MIN, UNLIMITED_MAX, ELE_CHANGE_BY, startingValue, true);
		}

		private double calculateMaxDiff(double[,] elevations)
		{
			double max = 0.0;
			for (int x = 0; x < this.x; x++){
				for (int z = 0; z < this.z; z++){
					Coordinates coord = new Coordinates(x, z);
                    List<Coordinates> coorAround = coord.getCoordinatesAround();
					double diffSum = 0.0;
					foreach (Coordinates coor in coorAround)
                    {
						diffSum += Math.Abs(elevations[coord.x, coord.z] - elevations[coor.x, coor.z]);   
                    }
					if(diffSum > max){
						max = diffSum;
					}
				}
			}
			return Math.Round(max, ROUND_TO);
		}

		private double calculateOceanPercentage(Coordinates coordinates, double[,] elevations)
		{
			List<Coordinates> coorAround = coordinates.getCoordinatesAround();
			double sum = 0.0;
			double negSum = 0.0;
			foreach (Coordinates coor in coorAround){
				double current = elevations[coor.x, coor.z];
				if (current < 0.0){
					negSum += current;
				}
				sum += Math.Abs(current);
			}
			return Math.Round(Math.Abs(negSum / sum), ROUND_TO);
		}

		private double calculateHillPercentage(Coordinates coordinates, double[,] elevations, double oceanPer)
		{
			if (oceanPer >= 1.00){
				return 0.0;
			}
			List<Coordinates> coorAround = coordinates.getCoordinatesAround();
            double diffSum = 0.0;
            foreach (Coordinates coor in coorAround)
            {
                double current = elevations[coor.x, coor.z];
				diffSum += Math.Abs(elevations[coordinates.x, coordinates.z] - current);
            }
			return Math.Round(Math.Abs(diffSum / maxDiff), ROUND_TO);
		}

		private int[,] generateLowTemps()
        {
			int startingValue = (int) Math.Round(randy.NextDouble() * STARTING_LOW_TEMP_RANGE + STARTING_LOW_TEMP_MIN, 0);
            return intLayerGenerator.GenerateIntLayer(LOW_TEMP_MIN, LOW_TEMP_MAX, TEMP_CHANGE_BY, startingValue, false);
        }
        
		private int[,] generateHighTemps(int[,] lowTemps)
        {
			int startingValue = (int) Math.Round(randy.NextDouble() * STARTING_HIGH_TEMP_RANGE + STARTING_HIGH_TEMP_MIN, 0);
			return intLayerGenerator.GenerateIntLayer(HIGH_TEMP_MIN, HIGH_TEMP_MAX, TEMP_CHANGE_BY, startingValue, false, lowTemps);
        }

		private int[,] generateSummerLength()
        {
			int startingValue = (int) Math.Round(randy.NextDouble() * STARTING_SUMMER_LENGTH_RANGE + STARTING_SUMMER_LENGTH_MIN, 0);
			return intLayerGenerator.GenerateIntLayer(SUMMER_LENGTH_MIN, SUMMER_LENGTH_MAX, TEMP_CHANGE_BY, startingValue, false);
        }
        
		private double[,] generateVariance()
        {
            double startingValue = randy.NextDouble() * VARIANCE_MAX + VARIANCE_MIN;
            return doubleLayerGenerator.GenerateWorldLayer(VARIANCE_MIN, VARIANCE_MAX, VARIANCE_CHANGE_BY, startingValue, false);
        }
        
		private double[][,] generateHumidities()
        {
			double[][,] humidities = new double[Precipitation.HUMIDITY_CROSS_SECTIONS][,];
			for (int i = 0; i < Precipitation.HUMIDITY_CROSS_SECTIONS; i++){
				double startingValue = randy.NextDouble() * HUMIDITY_MAX + HUMIDITY_MIN;
				humidities[i] = doubleLayerGenerator.GenerateWorldLayer(HUMIDITY_MIN, HUMIDITY_MAX, HUMIDITY_CHANGE_BY, startingValue, false);
			}
			return humidities;
        }

		private double[] convertThisTilesHumiditiesToArray(int x, int z, double[][,] humidities)
		{
			double[] array = new double[Precipitation.HUMIDITY_CROSS_SECTIONS];
			for (int i = 0; i < Precipitation.HUMIDITY_CROSS_SECTIONS; i++){
				array[i] = Math.Round(humidities[i][x, z], ROUND_TO);
			}
			return array;
		}

		private Direction.CardinalDirections[,] calculateDownStreams(double[,] elevations, out double[,] flowRates)
		{
			Direction.CardinalDirections[,] downstreams = new Direction.CardinalDirections[this.x, this.z];
			flowRates = new double[this.x, this.z];
			for (int x = 0; x < this.x; x++){
				for (int z = 0; z < this.z; z++)
                {
					Coordinates myPosition = new Coordinates(x, z);
					List<Direction.CardinalDirections> directionsAround = myPosition.getCardinalDirectionsAround();
					double lowest = elevations[x, z];
					Direction.CardinalDirections flowTo = Direction.CardinalDirections.none;
					foreach(Direction.CardinalDirections direction in directionsAround)
					{
						Coordinates coor = myPosition.findCoordinatesInCardinalDirection(direction);
						if(elevations[coor.x, coor.z] < lowest)
						{
							lowest = elevations[coor.x, coor.z];
							flowTo = direction;
						}
					}
					downstreams[x, z] = flowTo;
					flowRates[x, z] = Math.Round((elevations[x, z] - lowest) * FLOW_RATE_MULT, ROUND_TO);
                }
			}

			return downstreams;
		}

		private List<Direction.CardinalDirections> getUpstreamFromDownstream(int x, int z, Direction.CardinalDirections[,] downstreamDirections)
		{
			List<Direction.CardinalDirections> upstream = new List<Direction.CardinalDirections>();
			Coordinates myPosition = new Coordinates(x, z);
			List<Direction.CardinalDirections> directionAroundMe = myPosition.getCardinalDirectionsAround();
			foreach(Direction.CardinalDirections direction in directionAroundMe)
			{
				Coordinates coor = myPosition.findCoordinatesInCardinalDirection(direction);
				if(Direction.isOpposite(downstreamDirections[coor.x, coor.z], direction)){
					upstream.Add(direction);
				}
			}

			return upstream;
		}

        private void generateYearOfWeather(int year)
		{
			// Best plan seems to be update the year to be the current one held in file
			// but if habitat sum != 100.0 then slowly build it.
			double[][,] rainDays = new double[WorldDate.DAYS_PER_YEAR][,];
			double[][,] snowfall = new double[WorldDate.DAYS_PER_YEAR][,];
			for (int day = 1; day <= WorldDate.DAYS_PER_YEAR; day++)
			{
				rainDays[day - 1] = generateDayOfTempsAndPrecipitation(year, day, out snowfall[day - 1]);
			}
            // SKIP THIS IF OCEAN
			double[,] lastSnowOfYear;
			double[,] lastWaterOfYear = getLastDayOfPreviousYear(year, out lastSnowOfYear);
			double[][,] snowCover = new double[WorldDate.DAYS_PER_YEAR][,];
			double[][,] surfaceWater = calculateWaterData(rainDays, snowfall, out snowCover, lastSnowOfYear, lastWaterOfYear);
			saveYearOfWeather(year, rainDays, snowfall, snowCover, surfaceWater);
		}
            
        private double[,] generateDayOfTempsAndPrecipitation(int year, int day, out double[,] snowfall)
		{
			double startingValue = randy.NextDouble() * HUMIDITY_MAX;
			double[,] dailyRandomMap = doubleLayerGenerator.GenerateWorldLayer(startingValue, 0.0, HUMIDITY_MAX, RAINFALL_MULTIPLIER, true);
            double[,] precip = new double[x, z];
			snowfall = new double[x, z];
            // now i need to subtract the arrays and divide by humidities to generate a percentage
            // Then if > 0, multiply that percentage by a rainfall constant. (perhaps after squaring or cubing the percentage?
            for (int x = 0; x < this.x; x++)
            {
                for (int z = 0; z < this.z; z++)
                {
					if (day == 1)
					{
						// GENERATES THE DAILY TEMPS
						tileArray[x, z].temperatures.generateYearsTemps(year);
					}
					double humidityDiff = Math.Max((dailyRandomMap[x, z] + tileArray[x, z].precipitation.getHumidity(day) - HUMIDITY_MAX) / HUMIDITY_MAX, 0.0);
					if (!humidityDiff.Equals(0.0))
                    {
						double precipitation = Math.Round(Math.Pow(humidityDiff, RAINFALL_POWER) * RAINFALL_MULTIPLIER, ROUND_TO);
						if (tileArray[x, z].temperatures.dailyTemps.days[day - 1] > 32){
							precip[x, z] = precipitation;
						} else {
							snowfall[x, z] = precipitation;
						}
                    }
                }
            }

            return precip;
		}
        
		private double[][,] calculateWaterData(double[][,] rainDays, double[][,] snowfall, out double[][,] snowCover, double[,] lastSnowOfYear, double[,] lastWaterOfYear)
		{
			double[][,] surfaceWater = new double[WorldDate.DAYS_PER_YEAR][,];
			snowCover = new double[WorldDate.DAYS_PER_YEAR][,];
			for (int day = 1; day <= WorldDate.DAYS_PER_YEAR; day++)
			{
				surfaceWater[day - 1] = new double[this.x, this.z];
				snowCover[day - 1] = new double[this.x, this.z];
				for (int x = 0; x < this.x; x++)
				{
					for (int z = 0; z < this.z; z++)
					{
						if (x == 0 && z == 0){
							Console.Write(rainDays[day - 1][x, z] + " - ");
						}
						if (tileArray[x, z].terrain.oceanPercent < 1.0)
						{
							double yesterdaysSnow;
							double yesterdaysWater;

							if (day == 1)
							{
								yesterdaysSnow = lastSnowOfYear[x, z];
								yesterdaysWater = lastWaterOfYear[x, z];
							}
							else
							{
								yesterdaysSnow = snowCover[WorldDate.getYesterday(day) - 1][x, z];
								yesterdaysWater = surfaceWater[WorldDate.getYesterday(day) - 1][x, z];
							}

							int todaysTemp = tileArray[x, z].temperatures.dailyTemps.days[day - 1];

							double snowMelt = Math.Min(Math.Max((todaysTemp - FREEZING_POINT) * MELT_CONST, 0.0), yesterdaysSnow);
							snowCover[day - 1][x, z] = Math.Round(Math.Max(yesterdaysSnow + snowfall[day - 1][x, z] - snowMelt, 0.0), ROUND_TO);
							double humid = tileArray[x, z].precipitation.getHumidity(day);
							double flowAway = 0.0;
							if (tileArray[x, z].rivers.flowDirection != Direction.CardinalDirections.none){
								flowAway = tileArray[x, z].rivers.flowRate;
							}
                            
							double waterRemoved = calculateSoilAbsorption(x, z) + flowAway + calculateEvaporation(yesterdaysWater, todaysTemp, humid, determineWeather(rainDays[day - 1][x, z], humid));
							double waterGained = rainDays[day - 1][x, z] + getUpstreamFlow(day, x, z, surfaceWater, lastWaterOfYear) + snowMelt;
							surfaceWater[day - 1][x, z] = Math.Round(Math.Max(yesterdaysWater - waterRemoved + waterGained, 0.0), ROUND_TO);
						}
					}
				}
			}
			return surfaceWater;
		}
        
		private double calculateSoilAbsorption(int x, int z)
        {
			return ((randy.NextDouble() + .2) * (1.0 - tileArray[x, z].terrain.oceanPercent) * (1.0 - tileArray[x, z].terrain.hillPercent));
        }

		private double calculateEvaporation(double currentWater, int temp, double humidity, string weather)
        {
            double xs;
            double x;
            double pws;
            double pw;
			double evaporation;
            double multiplier = 1.0;

            switch (weather)
            {
                case "cloudy":
                    multiplier = 16.0;
                    break;
                case "sunny":
                    multiplier = 48.0;
                    break;
            }
            pws = Math.Exp(77.345 + 0.0057 * ((temp + 459.67) * (5.0 / 9.0)) - 7235.0 / ((temp + 459.67) * (5.0 / 9.0))) / (Math.Pow((temp + 459.67) * (5.0 / 9.0), 8.2));
            xs = 0.62198 * pws / (101325.0 - pws);
            if (weather != "rainy")
            {
                pw = (humidity / multiplier) * pws;
            }
            else
            {
                pw = pws;
            }
            x = 0.62198 * pw / (101325.0 - pw);
			evaporation = (float)(((1260.0 * 24.0 * Math.Sqrt(2.0 * currentWater * 23.6) / Math.Pow(6.0, .25)) * (xs - x)) / 23600.0);

            return evaporation;
        }
        
		private string determineWeather(double rainfall, double humidity)
        {
            string weather;
            if (rainfall > 0)
            {
                weather = "rainy";
            }
            else
            {
                weather = determineCloudy(humidity);
            }

            return weather;
        }

        // Determine if it is cloudy or not
		private string determineCloudy(double humidity)
        {
            string weather;
            double prob = Math.Pow(0.5, humidity / 5.0);

            if (randy.NextDouble() < prob)
            {
                weather = "sunny";
            }
            else
            {
                weather = "cloudy";
            }

            return weather;
        }

		private double getUpstreamFlow(int day, int x, int z, double[][,] surfaceWater, double[,] lastWaterOfYear)
		{
			double upstreamFlow = 0.0;
			List<Direction.CardinalDirections> directions = tileArray[x, z].rivers.upstreamDirections;
			Coordinates myPosition = new Coordinates(x, z);
            foreach (Direction.CardinalDirections direction in directions)
			{
                Coordinates coordinates = myPosition.findCoordinatesInCardinalDirection(direction);
				double flow = tileArray[coordinates.x, coordinates.z].rivers.flowRate;
				double yesterdayUpstreamWater;

				if (day == 1){
					yesterdayUpstreamWater = lastWaterOfYear[coordinates.x, coordinates.z];
				} else {
					yesterdayUpstreamWater = surfaceWater[WorldDate.getYesterday(day) - 1][coordinates.x, coordinates.z];
				}

				if (yesterdayUpstreamWater > flow)
				{
					upstreamFlow += flow;
				} else {
					upstreamFlow += yesterdayUpstreamWater;
				}
			}

			return upstreamFlow;
		}

		private void saveYearOfWeather(int year, double[][,] rainDays, double[][,] snowfall, double[][,] snowCover, double[][,] surfaceWater)
		{
			for (int x = 0; x < this.x; x++){
				for (int z = 0; z < this.z; z++){
					double[] rain = new double[WorldDate.DAYS_PER_YEAR];
					double[] snowF = new double[WorldDate.DAYS_PER_YEAR];
					double[] snowC = new double[WorldDate.DAYS_PER_YEAR];
					double[] volume = new double[WorldDate.DAYS_PER_YEAR];
					for (int day = 0; day < WorldDate.DAYS_PER_YEAR; day++){
						rain[day] = Math.Round(rainDays[day][x, z], ROUND_TO);
						snowF[day] = snowfall[day][x, z];
						snowC[day] = snowCover[day][x, z];
						volume[day] = surfaceWater[day][x, z];
					}
                    // GENERATES THE NEW DAILY RAIN & DAILY VOLUME
					tileArray[x, z].precipitation.setDailyRain(new DailyRain(year, rain, snowF, snowC));
					tileArray[x, z].rivers.dailyVolume = new DailyVolume(year, volume);
					// HABITAT UPDATE
					double avgTemp = tileArray[x, z].temperatures.dailyTemps.getAvgTemp();
					double totalPrecipitation = tileArray[x, z].precipitation.getRainForYear();
					double avgRiverLevel = tileArray[x, z].rivers.dailyVolume.getAverageWaterLevel();
					double oceanPer = tileArray[x, z].terrain.oceanPercent;
					tileArray[x, z].habitats.growHabitats(avgTemp, totalPrecipitation, avgRiverLevel, oceanPer.Equals(1.0), determineIsIceSheet(snowC));
				}
			}
		}

		private double[,] getLastDayOfPreviousYear(int year, out double[,] lastSnowOfYear)
		{
			// THIS NEEDS TO ACTUALLY GRAB IT FROM THE PREVIOUS YEAR OF WEATHER AND WE SHOULD BE GOOD TO LOOP THE YEARS
			lastSnowOfYear = new double[this.x, this.z];
			double[,] lastWaterOfYear = new double[this.x, this.z];
            if (year > 1)
			{
				for (int x = 0; x < this.x; x++){
					for (int z = 0; z < this.z; z++){
						lastWaterOfYear[x, z] = tileArray[x, z].rivers.dailyVolume.volume[WorldDate.DAYS_PER_YEAR - 1];
						lastSnowOfYear[x, z] = tileArray[x, z].precipitation.dailyRain.snowCover[WorldDate.DAYS_PER_YEAR - 1];
					}
				}
			}

			return lastWaterOfYear;
		}

		private bool determineIsIceSheet(double[] snowC){
			for (int day = 0; day < snowC.Length; day++){
				if (snowC.Equals(0.0)){
					return false;
				}
			}
			return true;
		}

    }
}
