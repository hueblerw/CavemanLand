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
			world = new World(50, 50);
		}

        public void loadGeneralFiles()
		{
			string json = loadJsonFileToString(@"Animal.json");
            World.animalSpecies = JsonConvert.DeserializeObject<Animal[]>(json);

            json = loadJsonFileToString(@"Plant.json");
            World.plantSpecies = JsonConvert.DeserializeObject<Plant[]>(json);
		}

		private string loadJsonFileToString(string pathname)
		{
			return File.ReadAllText(pathname);
		}
    }
}
