using Newtonsoft.Json;
using System;
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
        
        public World generateWorld(int x, int z)
		{
			loadGeneralFiles();
			world = new World(x, z);
			return world;
		}

        public void loadGeneralFiles()
		{
			string json = loadJsonFileToString("Animal.json");
			Animal[] animals = JsonConvert.DeserializeObject<Animal[]>(json);
			World.setAnimalSpecies(animals);

            json = loadJsonFileToString("Plants.json");
			Plant[] plants = JsonConvert.DeserializeObject<Plant[]>(json);
			World.setPlantSpecies(plants);
		}

		public void saveWorld(string worldName){
			world.saveGameFiles(worldName);
		}

		public World GetWorld()
		{
			return world;
		}

		public void printTileInfo(int x, int z){
			WorldDate date = world.currentDate;
			Console.WriteLine(date);
			Console.WriteLine(world.getTileArray()[x, z].getTileInfo(date.day));
		}

		private string loadJsonFileToString(string pathname)
		{
			return MyJsonFileInteractor.loadJsonFileToString(DATA_FILE_PATH + pathname);
		}
    }
}
