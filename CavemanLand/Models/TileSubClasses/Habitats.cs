using System.Collections.Generic;
using System;

namespace CavemanLand.Models.TileSubClasses
{
    public class Habitats
    {
		public const int NUMBER_OF_HABITATS = 14;

		public double[] typePercents;
		public double[] currentLevel;
		public double[] gameCurrentLevel;

		private Dictionary<int, string> habitatMapping;
		private Random randy;

		public Habitats()
        {
			this.typePercents = new double[NUMBER_OF_HABITATS];
			habitatMapping = createMapping();
			this.currentLevel = new double[NUMBER_OF_HABITATS];
			this.gameCurrentLevel = currentLevel;
			randy = new Random();
        }

        private double generateRandomLevel()
		{
			return randy.NextDouble() * 10.0;
		}

		private Dictionary<int, string> createMapping()
		{
			Dictionary<int, string> mapping = new Dictionary<int, string>();
			mapping.Add(0, "Artic Desert");
			mapping.Add(1, "Tundra");
			mapping.Add(2, "Boreal");
			mapping.Add(3, "Artic Marsh");
			mapping.Add(4, "Desert");
			mapping.Add(5, "Plains");
			mapping.Add(6, "Forest");
			mapping.Add(7, "Swamp");
			mapping.Add(8, "Hot Desert");
			mapping.Add(9, "Savannah");
			mapping.Add(10, "Monsoon Forest");
			mapping.Add(11, "Rainforest");
			mapping.Add(12, "Ice Sheet");
			mapping.Add(13, "Ocean");

			return mapping;
		}
    }
}
