using NUnit.Framework;
using Newtonsoft.Json;
using System.IO;
using System;
using CavemanLand.Models;
using CavemanLand.Controllers;

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

			string reConvertedJson = JsonConvert.SerializeObject(World.plantSpecies);
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

            string reConvertedJson = JsonConvert.SerializeObject(World.animalSpecies);
            string simplifiedJson = simplifyJson(json);
            Assert.AreEqual(simplifiedJson, reConvertedJson);
			Console.WriteLine("Correct Animal Json Loaded To Controller!");
        }

		private string loadJsonFileToString(string pathname)
        {
			return File.ReadAllText(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/DataFiles/" + pathname);
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
