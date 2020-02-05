using NUnit.Framework;
using CavemanLand.Utility;
using CavemanLand.Models.TileSubClasses;

namespace CavemanLand.UnitTests
{
    [TestFixture()]
    public class DailyVolumeUnitTestClass
    {
        [Test()]
        public void allZeroReturnsDrysOut()
        {
			double[] volume = new double[WorldDate.DAYS_PER_YEAR];
			DailyVolume dailyVolume = new DailyVolume(1, volume);
			Assert.True(dailyVolume.doesItDryOut());
        }

		[Test()]
        public void allHaveValueDoesntDryOut()
        {
            double[] volume = new double[WorldDate.DAYS_PER_YEAR];
			for (int d = 0; d < WorldDate.DAYS_PER_YEAR; d++)
			{
				volume[d] = 1.5;
			}
            DailyVolume dailyVolume = new DailyVolume(1, volume);
            Assert.False(dailyVolume.doesItDryOut());
        }

		[Test()]
        public void OneZeroDrysOut()
        {
            double[] volume = new double[WorldDate.DAYS_PER_YEAR];
            for (int d = 0; d < WorldDate.DAYS_PER_YEAR; d++)
            {
                volume[d] = 1.5;
            }
			volume[94] = 0.0;
            DailyVolume dailyVolume = new DailyVolume(1, volume);
            Assert.True(dailyVolume.doesItDryOut());
        }
    }
}
