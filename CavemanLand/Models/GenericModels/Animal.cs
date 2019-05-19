using System.Collections.Generic;

namespace CavemanLand.Models.GenericModels
{
    public class Animal
    {
		public string name;
		public int[] formsHerds = new int[2];
		public List<string> habitats;
		public List<int> abundance;
		public List<string> foodType;
		public double foodEaten;
		public int defense;
		public int attack;
		public int[] temperatureTolerance = new int[2];
		public double foodPerAnimal;
		public double weightPerUnit;
		public List<string> production;
		public List<double> productionPerUnit;
		public bool isRidable;
		public bool isBurden;

        public Animal()
        {
        }

		public override string ToString()
        {
            return name;
        }

    }
}
