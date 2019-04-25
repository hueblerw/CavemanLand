namespace CavemanLand.Models
{
    public class World
    {
		public static Animal[] animalSpecies;
		public static Plant[] plantSpecies;

		private int x;
		private int z;

        public World(int x, int z)
        {
			this.x = x;
			this.z = z;
        }
    }
}
