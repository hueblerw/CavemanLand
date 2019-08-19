using System;
using System.Collections.Generic;
using System.Linq;

namespace CavemanLand.Utility
{
    public class Direction
    {
		public string direction;
		private static string[] possibleCardinalValues = { "up", "down", "left", "right" };
		private static string[] possibleValues = { "up", "down", "left", "right", "upper_left", "upper_right", "lower_left", "lower_right" };
		private static List<Direction.AllDirections> allDirections;
		private static List<Direction.CardinalDirections> cardinalDirections;

        public static int getNumberOfDirections()
		{
			return possibleValues.Length;
		}

		public static int getNumberOfCardinalDirections()
        {
            return possibleCardinalValues.Length;
        }

        public static List<Direction.AllDirections> getAllDirections()
		{
			if (allDirections == null){
				allDirections = new List<AllDirections>();
                allDirections.Add(AllDirections.down);
                allDirections.Add(AllDirections.up);
                allDirections.Add(AllDirections.left);
                allDirections.Add(AllDirections.right);
                allDirections.Add(AllDirections.lower_left);
                allDirections.Add(AllDirections.lower_right);
                allDirections.Add(AllDirections.upper_left);
                allDirections.Add(AllDirections.upper_right);
			}         
			return allDirections;
		}

		public static List<Direction.CardinalDirections> getAllCardinalDirections()
        {
			if (cardinalDirections == null){
				cardinalDirections = new List<Direction.CardinalDirections>();
                cardinalDirections.Add(CardinalDirections.down);
                cardinalDirections.Add(CardinalDirections.up);
                cardinalDirections.Add(CardinalDirections.left);
				cardinalDirections.Add(CardinalDirections.right);
			} 
			return cardinalDirections;
        }

		public enum AllDirections
        {
            up,
            down,
            right,
            left,
            upper_left,
            upper_right,
            lower_left,
            lower_right
        };

		public enum CardinalDirections
        {
            up,
            down,
            right,
            left
        };

        public Direction()
        {
        }

		public Direction(string direction, bool isCardinal)
        {
			validateDirection(direction, isCardinal);
			this.direction = direction;
        }

        private void validateDirection(string dir, bool isCardinal)
		{
			if (isCardinal){
				if (!possibleCardinalValues.Contains(dir))
                {
                    throw new Exception("Invalid direction [" + dir + "]");
                }
			} else {
				if (!possibleValues.Contains(dir))
                {
                    throw new Exception("Invalid direction [" + dir + "]");
                }
			}
		}
    }
}
