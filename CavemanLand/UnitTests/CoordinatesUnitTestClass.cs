using NUnit.Framework;
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
			Coordinates coordinates3 = new Coordinates(0, 34);
            List<Coordinates> list3 = coordinates3.getCoordinatesAround();
			Coordinates coordinates4 = new Coordinates(49, 0);
			List<Coordinates> list4 = coordinates4.getCoordinatesAround();

			Assert.AreEqual("(20, 41), (20, 39), (19, 40), (21, 40), (19, 41), (21, 41), (19, 39), (21, 39), ", printCoordinateList(list));
			Assert.AreEqual("(0, 48), (1, 49), (1, 48), ", printCoordinateList(list2));
			Assert.AreEqual("(0, 35), (0, 33), (1, 34), (1, 35), (1, 33), ", printCoordinateList(list3));
			Assert.AreEqual("(49, 1), (48, 0), (48, 1), ", printCoordinateList(list4));
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
			Coordinates coord2 = coordinates2.findCoordinatesInDirection(Direction.AllDirections.left);
            
			Assert.AreEqual(new Coordinates(20, 39), coord);
			Assert.AreEqual(new Coordinates(19, 50), coord2);
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
