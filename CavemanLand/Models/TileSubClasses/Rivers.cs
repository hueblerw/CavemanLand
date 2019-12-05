using CavemanLand.Utility;
using System.Collections.Generic;

namespace CavemanLand.Models.TileSubClasses
{
    public class Rivers
    {
		public Direction.CardinalDirections flowDirection;
		public List<Direction.CardinalDirections> upstreamDirections;
		public double flowRate;
		public DailyVolume dailyVolume;

        public Rivers()
        {
        }

		public Rivers(Direction.CardinalDirections flowDirection, List<Direction.CardinalDirections> upstreamDirections, double flowRate)
        {
			this.flowDirection = flowDirection;
			this.upstreamDirections = upstreamDirections;
			this.flowRate = flowRate;
        }

		public string printUpstreamDirections()
		{
			string output = "";
			foreach(Direction.CardinalDirections dir in upstreamDirections)
			{
				output += dir + ", ";
			}
			return output;
		}
    }
}
