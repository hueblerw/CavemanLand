namespace CavemanLand.Models.TileSubClasses
{
    public class Habitats
    {
		public double[] typePercents;
		public double[] currentLevel;
		public double[] gameCurrentLevel;

        public Habitats()
        {
        }

		public Habitats(double[] typePercents, double[] currentLevel, double[] gameCurrentLevel)
        {
			this.typePercents = typePercents;
			this.currentLevel = currentLevel;
			this.gameCurrentLevel = gameCurrentLevel;
        }
    }
}
