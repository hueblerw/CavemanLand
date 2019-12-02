using System.Collections.Generic;

namespace CavemanLand.Models.TileSubClasses
{
    public class Minerals
    {
		public Dictionary<string, double> surface;
		public Dictionary<string, double> mineable;

        public Minerals()
        {
        }

		public Minerals(double surfaceStone, double mineableStone)
        {
			surface.Add("stone", surfaceStone);
			mineable.Add("stone", mineableStone);
        }

		public override string ToString()
		{
			return base.ToString();
		}

	}
}
