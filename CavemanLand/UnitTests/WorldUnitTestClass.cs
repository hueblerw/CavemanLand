using NUnit.Framework;
using CavemanLand.Models;
using CavemanLand.Utility;
using System;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class WorldUnitTestClass
    {
        [Test()]
        public void TestCase()
        {
			World world = new World(50, 50);
			world.saveGameFiles();
			// verify that this works correctly.
			verifyExpectedJson(@"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/UnitTests/Mocks/worldFileMock.json", @"/Users/williamhuebler/GameFiles/CavemanLand/CavemanLand/SaveFiles/WorldA-worldFile.json");
        }

        private void verifyExpectedJson(string expectedPath, string actualPath)
		{
			string expectedJson = MyJsonFileInteractor.loadJsonFileToString(expectedPath);
			string actualJson = MyJsonFileInteractor.loadJsonFileToString(actualPath);
			Assert.AreEqual(simplifyJson(expectedJson), actualJson);
			Console.Write("File is correctly saved!");
		}

		// removes tabs and line break characters to make comparing jsons accurate.
        private string simplifyJson(string json)
        {
            string newjson = json.Replace("\t", "");
			newjson = newjson.Replace("\r", "");
            newjson = newjson.Replace("\n", "");
            newjson = newjson.Replace(", ", ",");
            newjson = newjson.Replace(": ", ":");
            return newjson;
        }
       
    }
}
