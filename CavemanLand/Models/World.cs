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
		private const double UNLIMITED_MIN = -1000.0;
		private const double UNLIMITED_MAX = -UNLIMITED_MIN;
            // Elevation
    		private const double STARTING_ELE_RANGE = 10.0;
            private const double STARTING_ELE_MIN = -5.0;
            private const double ELE_CHANGE_BY = 2.0;
            
        // General Constants
		public const int ROUND_TO = 2;
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

        public World(int x, int z)
        {
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
        }
        
        public string tileArrayToJson()
		{
			return JsonConvert.SerializeObject(tileArray);
		}

		public void saveGameFiles()
		{
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

		public void generateNewYear()
		{
			throw new NotImplementedException();
		}

		public Tile[,] getTileArray()
		{
			return tileArray;
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
            
			// Populate layers
			for (int x = 0; x < this.x; x++)
			{
				for (int z = 0; z < this.z; z++)
				{
					tiles[x, z] = new Tile(x, z);
					double oceanPer = calculateOceanPercentage(tiles[x, z].coor, elevations);
					tiles[x, z].terrain = new Terrain(elevations[x, z], oceanPer, calculateHillPercentage(tiles[x, z].coor, elevations, oceanPer));               
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
    }
}
