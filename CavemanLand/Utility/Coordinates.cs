namespace CavemanLand.Utility
{
    public class Coordinates
    {
		public int x;
		public int z;

        public Coordinates(int x, int z)
        {
			this.x = x;
			this.z = z;
        }

		public override string ToString()
		{
			return "(" + x + ", " + z + ")";
		}
	}
}
