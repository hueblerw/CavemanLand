using System;
using System.Collections.Generic;

namespace CavemanLand.Utility
{
    public class Coordinates
    {
		private static int worldX;
		private static int worldZ;
		public int x;
		public int z;

        public Coordinates(int x, int z)
        {
			this.x = x;
			this.z = z;
        }

        public static void setWorldSize(int x, int z)
		{
			worldX = x;
			worldZ = z;
		}

		public override string ToString()
		{
			return "(" + x + ", " + z + ")";
		}

		public bool Equals(Coordinates coordinates)
        {
            return (this.x == coordinates.x && this.z == coordinates.z);
        }

		public List<Coordinates> getCoordinatesAround()
        {
            List<Coordinates> coordinates = new List<Coordinates>();
            List<Direction.AllDirections> array = getLegalDirectionsAround(findDirectionsOffWorldMap(this));
            foreach (Direction.AllDirections direction in array)
            {
                coordinates.Add(findCoordinatesInDirection(direction));
            }
            return coordinates;
        }

        public Coordinates randomDirectionAround()
        {
            // get directions that are forbidden base on coordinates.
            List<Direction.AllDirections> directionsToRemove = findDirectionsOffWorldMap(this);
            // get a random allowable direction
           Direction.AllDirections direction = getRandomDirection(directionsToRemove);
            // Convert that into the new coordinates
            return findCoordinatesInDirection(direction);
        }

        public Coordinates findCoordinatesInDirection(Direction.AllDirections direction)
        {
            switch (direction)
            {
                case Direction.AllDirections.up:
                    return new Coordinates(x, z - 1);
				case Direction.AllDirections.down:
                    return new Coordinates(x, z + 1);
				case Direction.AllDirections.left:
                    return new Coordinates(x - 1, z);
				case Direction.AllDirections.right:
                    return new Coordinates(x + 1, z);
				case Direction.AllDirections.upper_left:
                    return new Coordinates(x - 1, z - 1);
				case Direction.AllDirections.upper_right:
                    return new Coordinates(x + 1, z - 1);
				case Direction.AllDirections.lower_left:
                    return new Coordinates(x - 1, z + 1);
				case Direction.AllDirections.lower_right:
                    return new Coordinates(x + 1, z + 1);
            }
            throw new Exception("Invalid Direction supplied!");
        }

        private List<Direction.AllDirections> findDirectionsOffWorldMap(Coordinates coor)
        {
            List<Direction.AllDirections> forbiddenDirections = new List<Direction.AllDirections>();

            if (coor.x == 0)
            {
				forbiddenDirections.Add(Direction.AllDirections.left);
				forbiddenDirections.Add(Direction.AllDirections.lower_left);
				forbiddenDirections.Add(Direction.AllDirections.upper_left);
            }
            if (coor.x == worldX - 1)
            {
				forbiddenDirections.Add(Direction.AllDirections.right);
				forbiddenDirections.Add(Direction.AllDirections.lower_right);
				forbiddenDirections.Add(Direction.AllDirections.upper_right);
            }
            if (coor.z == 0)
            {
				forbiddenDirections.Add(Direction.AllDirections.up);
				forbiddenDirections.Add(Direction.AllDirections.upper_right);
				forbiddenDirections.Add(Direction.AllDirections.upper_left);
            }
            if (coor.z == worldZ - 1)
            {
				forbiddenDirections.Add(Direction.AllDirections.down);
				forbiddenDirections.Add(Direction.AllDirections.lower_right);
				forbiddenDirections.Add(Direction.AllDirections.lower_left);
            }

            return forbiddenDirections;
        }

		private Direction.AllDirections getRandomDirection(List<Direction.AllDirections> directionsToRemove)
		{
			Random randy = new Random();
			List<Direction.AllDirections> legalList = getLegalDirectionsAround(directionsToRemove);
			int index = randy.Next(legalList.Count);
			return legalList[index];
		}

		private List<Direction.AllDirections> getLegalDirectionsAround(List<Direction.AllDirections> directionsToRemove)
		{
			List<Direction.AllDirections> legalList = Direction.getAllDirections();
            foreach (Direction.AllDirections direction in directionsToRemove)
            {
                legalList.Remove(direction);
            }
			return legalList;
		}
	}
}
