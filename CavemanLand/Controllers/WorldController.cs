using System.IO;
using Newtonsoft.Json;
using CavemanLand.Models;

namespace CavemanLand.Controllers
{
    public class WorldController
	{
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
			return File.ReadAllText(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/DataFiles/" + pathname);
		}
    }
}
