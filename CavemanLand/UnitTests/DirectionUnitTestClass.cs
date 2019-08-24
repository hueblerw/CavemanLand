using NUnit.Framework;
using CavemanLand.Utility;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class DirectionUnitTestClass
    {
        [Test()]
        public void VerifyIsOppositeTest()
        {
			Assert.True(Direction.isOpposite(Direction.CardinalDirections.up, Direction.CardinalDirections.down));
			Assert.True(Direction.isOpposite(Direction.CardinalDirections.down, Direction.CardinalDirections.up));
			Assert.True(Direction.isOpposite(Direction.CardinalDirections.left, Direction.CardinalDirections.right));
			Assert.True(Direction.isOpposite(Direction.CardinalDirections.right, Direction.CardinalDirections.left));

			Assert.False(Direction.isOpposite(Direction.CardinalDirections.up, Direction.CardinalDirections.right));
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.up, Direction.CardinalDirections.left));
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.down, Direction.CardinalDirections.right));
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.down, Direction.CardinalDirections.left));
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.up, Direction.CardinalDirections.up));
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.down, Direction.CardinalDirections.down));
            
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.right, Direction.CardinalDirections.up));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.right, Direction.CardinalDirections.down));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.right, Direction.CardinalDirections.right));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.left, Direction.CardinalDirections.up));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.left, Direction.CardinalDirections.down));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.left, Direction.CardinalDirections.left));
            
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.up, Direction.CardinalDirections.none));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.none, Direction.CardinalDirections.up));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.down, Direction.CardinalDirections.none));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.none, Direction.CardinalDirections.down));
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.right, Direction.CardinalDirections.none));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.none, Direction.CardinalDirections.right));
			Assert.False(Direction.isOpposite(Direction.CardinalDirections.left, Direction.CardinalDirections.none));
            Assert.False(Direction.isOpposite(Direction.CardinalDirections.none, Direction.CardinalDirections.left));

			Assert.False(Direction.isOpposite(Direction.CardinalDirections.none, Direction.CardinalDirections.none));
        }
    }
}
