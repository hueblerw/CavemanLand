using System;

namespace CavemanLand.Models.TileSubClasses
{
    public class SubHabitat
    {
		// Constants
		private const double GRASS_PER_TILE_CONSTANT = 0.4;

		private const double SWAMP_CONSTANT = .8;
		private const double DESERTGROWTHFACTOR = .3;
		private const double GRASSCALORIECONTENT = .067311605;

		private const double SEED_CONSTANT = .067311605;
		private const double PINE_SEED_CONSTANT = 2.777777778 * 0.003856682;
		private const double OAK_SEED_CONSTANT = 2.023809524 * 0.003020609;
		private const double TROPICAL_SEED_CONSTANT = SEED_CONSTANT;

		private const double TROPICALEAFGROWTH = 1.2;
		private const double ARTICLEAFGROWTH = 0.8;

		private const double FORESTLEAVESCONSTANT = 0.0019283411;
		private const double PINENEEDLECONSTANT = .000650815125;
		private const double SHRUB_CONSTANT = 45.0 * 0.004338767;
  
		public static double getGrazing(double grass)
        {
			return Math.Round(grass * GRASSCALORIECONTENT, World.ROUND_TO);
        }

		public static int getTreesOnTile(double forestPercentage, double swampPercentage, double quality)
        {
			double trees = 0;
			double qualityfactor = quality * 200.0 + 500.0;
			trees += forestPercentage * qualityfactor;
			trees += swampPercentage * qualityfactor * SWAMP_CONSTANT;
            return (int) Math.Round(trees, 0);
        }

		// Return the grass number for today
		public static double getGrass(double quality, double plainsPercentage, double desertPercentage, double last5Rain, int temp)
        {
            // =(1.2-ABS(70-AA7)/70)*400*SUM($AF$1:$AF$3)*(($AJ$4-50)/200+1)+(1.2-ABS(70-AA7)/70)*400*(($AJ$4-50)/200+1)*SUM($AD$1:$AD$3)*0.3*(0.5+SUM(AB7:AB11)/10)
            double tempfactor = Math.Max((1.2 - Math.Abs(70.0 - temp) / 70.0), 0.0);
            // Calculate the grass
            double grass = 0.0;
			double qualityfactor = ((quality - 5.0) / 20.0 + 1.0);
			grass += tempfactor * 400 * plainsPercentage * qualityfactor * GRASS_PER_TILE_CONSTANT;
			grass += tempfactor * 400 * desertPercentage * qualityfactor * DESERTGROWTHFACTOR * (.5 + (last5Rain / 10.0)) * GRASS_PER_TILE_CONSTANT;
			return Math.Round(grass, World.ROUND_TO);
        }

		public static double getSeeds(double grass, int trees, double coldPercent, double normalPercent, double tropicalPercent)
        {
			double seeds = grass * SEED_CONSTANT;
			if (trees != 0){
				double seedMultiplier = (normalPercent * OAK_SEED_CONSTANT + coldPercent * PINE_SEED_CONSTANT + tropicalPercent * TROPICAL_SEED_CONSTANT);
				seeds += trees * seedMultiplier;
			}

			return Math.Round(seeds, World.ROUND_TO);
        }

		public static double getFoilage(double quality, int todayTemp, int trees, double coldPercent, double normalPercent, double tropicalPercent)
        {
			double foilage = ((quality / 20.0) + .75) * SHRUB_CONSTANT * (normalPercent + coldPercent * ARTICLEAFGROWTH + tropicalPercent * TROPICALEAFGROWTH);
			if (trees != 0){
				double decidousBiomass = trees * FORESTLEAVESCONSTANT * (TROPICALEAFGROWTH * tropicalPercent + normalPercent);
				foilage += trees * PINENEEDLECONSTANT * ARTICLEAFGROWTH * coldPercent;
				if (todayTemp > 50)
					foilage += decidousBiomass;
                else
                {
                    if (todayTemp > 30)
                    {
						foilage += ((todayTemp - 30.0) / 20.0) * decidousBiomass;
                    }
                }
			}

			return Math.Round(foilage, World.ROUND_TO);
        }
    }
}
