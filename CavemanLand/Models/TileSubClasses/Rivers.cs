using CavemanLand.Utility;
using System.Collections.Generic;

namespace CavemanLand.Models.TileSubClasses
{
    public class Rivers
    {
		public Direction flowDirection;
		public List<Direction> upstreamDirections;
		public double flowRate;
		public DailyVolume dailyVolume;

        public Rivers()
        {
        }

		public Rivers(Direction flowDirection, List<Direction> upstreamDirections, double flowRate, DailyVolume dailyVolume)
        {
			this.flowDirection = flowDirection;
			this.upstreamDirections = upstreamDirections;
			this.flowRate = flowRate;
        }
    }
}
