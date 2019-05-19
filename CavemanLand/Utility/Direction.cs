using System;
using System.Linq;

namespace CavemanLand.Utility
{
    public class Direction
    {
		public string direction;
		private string[] possibleValues = { "up", "down", "left", "right" };

        public Direction()
        {
        }

		public Direction(string direction)
        {
			validateDirection(direction);
			this.direction = direction;
        }

        private void validateDirection(string dir)
		{
			if (!possibleValues.Contains(dir))
			{
				throw new Exception("Invalid direction [" + dir + "]");
			}
		}
    }
}
