using NUnit.Framework;
using CavemanLand.Utility;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class CoordinatesUnitTestClass
    {
        [Test()]
        public void CoordinatePrintCorrectlyTest()
        {
			Coordinates coordinates = new Coordinates(20, 40);
			Assert.AreEqual("(20, 40)", coordinates.ToString());
        }
    }
}
