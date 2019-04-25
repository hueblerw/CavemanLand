using NUnit.Framework;
using Newtonsoft.Json;
using System.IO;
using System;
using CavemanLand.Models;

namespace CavemanLand.UnitTests
{
	[TestFixture]
    public class WorldControllerTestClass
    {
		[Test]
        public void jsonToObjectTest()
        {
			string json = loadJsonFileToString("Plants.json");
            Plant[] plants = JsonConvert.DeserializeObject<Plant[]>(json);

			Assert.AreEqual(15, plants.Length);
			string reConvertedJson = JsonConvert.SerializeObject(plants[4]);
			Console.Write(reConvertedJson);

			json = loadJsonFileToString("Animal.json");
            Animal[] animals = JsonConvert.DeserializeObject<Animal[]>(json);
            
			Assert.AreEqual(28, animals.Length);
			reConvertedJson = JsonConvert.SerializeObject(plants[4]);
            Console.Write(reConvertedJson);
        }

		private string loadJsonFileToString(string pathname)
        {
			return File.ReadAllText(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/DataFiles/" + pathname);
        }
    }
}
