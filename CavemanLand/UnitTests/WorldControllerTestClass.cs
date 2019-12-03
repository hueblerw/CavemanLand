using NUnit.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CavemanLand.Models;
using CavemanLand.Models.GenericModels;
using CavemanLand.Controllers;
using CavemanLand.Utility;

namespace CavemanLand.UnitTests
{
	[TestFixture]
    public class WorldControllerTestClass
    {
		[Test]
        public void jsonToObjectAndBackTest()
        {
			string json = loadJsonFileToString("Plants.json");
            Plant[] plants = JsonConvert.DeserializeObject<Plant[]>(json);

			Assert.AreEqual(15, plants.Length);
			string reConvertedJson = JsonConvert.SerializeObject(plants);
			string simplifiedJson = simplifyJson(json);
			Assert.AreEqual(simplifiedJson, reConvertedJson);
			Console.WriteLine("Plants can convert in both directions!");

			json = loadJsonFileToString("Animal.json");
            Animal[] animals = JsonConvert.DeserializeObject<Animal[]>(json);
            
			Assert.AreEqual(28, animals.Length);
			reConvertedJson = JsonConvert.SerializeObject(animals);
			simplifiedJson = simplifyJson(json);
			Assert.AreEqual(simplifiedJson, reConvertedJson);
			Console.WriteLine("Animals can convert in both directions!");
        }

		[Test]
        public void correctPlantJsonLoaded()
        {
            string json = loadJsonFileToString("Plants.json");

			WorldController controller = new WorldController();
			controller.loadGeneralFiles();
            
			Plant[] array = convertPlantHashToArray(World.plantSpecies);
			string reConvertedJson = JsonConvert.SerializeObject(array);
			string simplifiedJson = simplifyJson(json);
            Assert.AreEqual(simplifiedJson, reConvertedJson);
			Console.WriteLine("Correct Plants Json Loaded To Controller!");
        }

		[Test]
        public void correctAnimalJsonLoaded()
        {
            string json = loadJsonFileToString("Animal.json");

            WorldController controller = new WorldController();
            controller.loadGeneralFiles();

			Animal[] array = convertAnimalHashToArray(World.animalSpecies);
            string reConvertedJson = JsonConvert.SerializeObject(array);
            string simplifiedJson = simplifyJson(json);
            Assert.AreEqual(simplifiedJson, reConvertedJson);
			Console.WriteLine("Correct Animal Json Loaded To Controller!");
        }

		[Test]
        public void timePrintOutTileInfo()
        {
			WorldController controller = new WorldController();
            controller.generateWorld(50, 50);
			Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
			controller.printTileInfo(10, 31);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("\n");
            Console.WriteLine("Print tile info time: " + ts);
            stopWatch.Reset();
        }

		private string loadJsonFileToString(string pathname)
        {
			return MyJsonFileInteractor.loadJsonFileToString(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/DataFiles/" + pathname);
        }
        
		private Plant[] convertPlantHashToArray(Dictionary<string, Plant> plantHash){
			Plant[] array = new Plant[plantHash.Count];
			int i = 0;
			foreach(KeyValuePair<string, Plant> pair in plantHash){
				array[i] = pair.Value;
				i++;
			}
			return array;
		}

		private Animal[] convertAnimalHashToArray(Dictionary<string, Animal> animalHash)
        {
			Animal[] array = new Animal[animalHash.Count];
            int i = 0;
			foreach (KeyValuePair<string, Animal> pair in animalHash)
            {
                array[i] = pair.Value;
                i++;
            }
            return array;
        }

        // removes tabs and line break characters to make comparing jsons accurate.
		private string simplifyJson(string json){
			string newjson = json.Replace("\t", "");
			newjson = newjson.Replace("\n", "");
			newjson = newjson.Replace(", ", ",");
			newjson = newjson.Replace(": ", ":");
			return newjson;
		}

    }
}
