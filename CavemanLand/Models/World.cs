using CavemanLand.Models.GenericModels;
using System.Collections.Generic;
using Newtonsoft.Json;
using CavemanLand.Utility;

namespace CavemanLand.Models
{
    public class World
    {
		private const string SAVE_FILE_LOCATION = @"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/SaveFiles/";

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

        public World(int x, int z)
        {
			this.x = x;
			this.z = z;
			currentDate = new WorldDate(1, 1);
			tileArray = generateTileArray();
			herds = new List<Herd>();
			tribes = new List<Tribe>();
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
			return null;
		}
    }
}
