using NUnit.Framework;
using System;
using System.Collections.Generic;
using CavemanLand.Utility;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class CoordinatesUnitTestClass
    {
        [Test()]
        public void CoordinatePrintCorrectlyTest()
        {
			Coordinates.setWorldSize(50, 50);
			Coordinates coordinates = new Coordinates(20, 40);
			Assert.AreEqual("(20, 40)", coordinates.ToString());
        }
        
		[Test()]
        public void CoordinateEqualFunction()
        {
			Coordinates.setWorldSize(50, 50);
            Coordinates coordinates = new Coordinates(20, 40);
			Coordinates coordinates2 = new Coordinates(20, 40);
			Coordinates coordinates3 = new Coordinates(37, 40);
			Coordinates coordinates4 = new Coordinates(20, 36);
			Coordinates coordinates5 = new Coordinates(2, 4);
            Assert.AreEqual(true, coordinates.Equals(coordinates2));
			Assert.AreEqual(false, coordinates.Equals(coordinates3));
			Assert.AreEqual(false, coordinates.Equals(coordinates4));
			Assert.AreEqual(false, coordinates.Equals(coordinates5));
        }
        
		[Test()]
		public void CoordinateGetTilesAroundTest()
        {
			Coordinates.setWorldSize(50, 50);
            Coordinates coordinates = new Coordinates(20, 40);
			List<Coordinates> list = coordinates.getCoordinatesAround();
			Coordinates coordinates2 = new Coordinates(0, 49);
            List<Coordinates> list2 = coordinates2.getCoordinatesAround();
			Coordinates coordinates3 = new Coordinates(49, 0);
            List<Coordinates> list3 = coordinates3.getCoordinatesAround();

			Console.WriteLine(printCoordinateList(list));
			Console.WriteLine(printCoordinateList(list2));
			Console.WriteLine(printCoordinateList(list3));

			Assert.AreEqual("(20, 41), (20, 39), (19, 40), (21, 40), (19, 41), (21, 41), (19, 39), (21, 39), ", printCoordinateList(list));
			Assert.AreEqual("(0, 48), (1, 49), (1, 48), ", printCoordinateList(list2));
			Assert.AreEqual("(20, 40)", printCoordinateList(list3));
        }
        
		[Test()]
		public void CoordinateRandomDirectionAroundTest()
        {
			Coordinates.setWorldSize(50, 50);
            Coordinates coordinates = new Coordinates(20, 40);
            List<Coordinates> list = coordinates.getCoordinatesAround();
			Coordinates randomCoordinates = coordinates.randomDirectionAround();
			Coordinates coordinates2 = new Coordinates(0, 50);
			List<Coordinates> list2 = coordinates2.getCoordinatesAround();
			Coordinates randomCoordinates2 = coordinates2.randomDirectionAround();
            Coordinates coordinates3 = new Coordinates(50, 0);
            List<Coordinates> list3 = coordinates3.getCoordinatesAround();
			Coordinates randomCoordinates3 = coordinates3.randomDirectionAround();

			Assert.Contains(randomCoordinates, list);
			Assert.Contains(randomCoordinates2, list2);
			Assert.Contains(randomCoordinates3, list3);
        }
        
		[Test()]
		public void FindCoordinatesInDirectionTest()
        {
			Coordinates.setWorldSize(50, 50);
            Coordinates coordinates = new Coordinates(20, 40);
			Coordinates coord = coordinates.findCoordinatesInDirection(Direction.AllDirections.up);
			Coordinates coordinates2 = new Coordinates(20, 50);
            Coordinates coord2 = coordinates.findCoordinatesInDirection(Direction.AllDirections.up);

			Assert.AreEqual(true, new Coordinates(20, 41).Equals(coord));
			Assert.AreEqual(null, coord2);
            
        }

        private string printCoordinateList(List<Coordinates> list)
		{
			string output = "";
			foreach(Coordinates coor in list){
				output += coor + ", ";
			}
			return output;
		}
        
    }
}
