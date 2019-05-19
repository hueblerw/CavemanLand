using Newtonsoft.Json;
using CavemanLand.Models;
using CavemanLand.Models.GenericModels;
using CavemanLand.Utility;

namespace CavemanLand.Controllers
{
    public class WorldController
	{
		private const string DATA_FILE_PATH = @"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/DataFiles/";

		private World world;

		public WorldController()
		{
		}

        public void generateWorld()
		{
			loadGeneralFiles();
			world = new World(50, 50);
		}

        public void loadGeneralFiles()
		{
			string json = loadJsonFileToString("Animal.json");
            World.animalSpecies = JsonConvert.DeserializeObject<Animal[]>(json);

            json = loadJsonFileToString("Plants.json");
            World.plantSpecies = JsonConvert.DeserializeObject<Plant[]>(json);
		}

		public World GetWorld()
		{
			return world;
		}

		private string loadJsonFileToString(string pathname)
		{
			return MyJsonFileInteractor.loadJsonFileToString(DATA_FILE_PATH + pathname);
		}
    }
}
