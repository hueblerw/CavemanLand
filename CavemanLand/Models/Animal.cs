using System.Collections.Generic;

namespace CavemanLand.Models
{
    public class Animal
    {
		private string name;
		private bool formsHerds;
		private List<string> habitats;
		private List<int> abundance;
		private List<string> foodType;
		private double foodEaten;
		private int defense;
		private int attack;
		private int[] temperatureTolerance = new int[2];
		private double foodPerAnimal;
		private double weightPerUnit;
		private List<string> production;
		private List<double> productionPerUnit;
		private bool isRidable;
		private bool isBurden;

        public Animal()
        {
        }
    }
}
