using System.Collections.Generic;
using CavemanLand.Models.TileSubClasses;
using CavemanLand.Utility;
using System;

namespace CavemanLand.Models.GenericModels
{
    public class Plant
    {
		public string name;
		public string type;
		public bool isFarmable;
		public List<string> habitats;
		public List<int> abundance;
		public int[] temperatureRange = new int[2];
		public double[] rainfallRange = new double[2];
		public int growthPeriod;
		public bool doesFrostKill;
		public string resourceProduced;
		public double unitPerHarvest;
		public double weightPerUnit;

        public Plant()
        {
        }

		public override string ToString(){
			return name;
		}

		// NOTE ***************
        // So far the crops can't grow early in the year because for that they need access to information from the previous year.
        // Implementation of that will be a bit tricky so I am saving it for later.
        // Also, these represent the number of new crops that grew today.  A scavenger would have access to the last x days worth of crops.
        // Calculate how much of a crop is present upon request
		public double[] getYearOfCrop(double habitatPercentage, DailyTemps dailyTemps, DailyRain dailyRain, DailyVolume dailyVolume)
        {
            double[] currentCrop = new double[WorldDate.DAYS_PER_YEAR];
            double percentGrowable = 1.0;
			// For each of the crops
			// If the crop can grow in the region return the crops store the crops returned value in the current crop array for today.
			int lastInvalidDay = -1;
			double rainSum = 0.0;
			bool isSomeCrop = false;
			for (int day = 0; day < WorldDate.DAYS_PER_YEAR; day++)
			{
				if (!dayTempAllowCrop(day, dailyTemps.days))
                {
					lastInvalidDay = day;
                }
				if (day - lastInvalidDay >= growthPeriod - 1 && dayRainAllowCrop(day, dailyRain.precip, dailyVolume.volume, out rainSum, out percentGrowable))
				{
					double cropMultiplier = (1.0 / ((80 - growthPeriod) * 100.0)) * 400.0 * habitatPercentage;
					currentCrop[day] = calculateCropQuality(day, rainSum, dailyTemps.days) * cropMultiplier * unitPerHarvest * percentGrowable;
					if (!isSomeCrop && !currentCrop[day].Equals(0.0)){
						isSomeCrop = true;
					}
				}
                
			}
         
			if (isSomeCrop)
			{
				return currentCrop;
			} else 
			{
				return null;
			}
        }


        // Determine if starting on today the previous days have a suitable temperature range.
        private bool dayTempAllowCrop(int day, int[] dailyTemps)
        {
			if (dailyTemps[day] < temperatureRange[0] - 10 || dailyTemps[day] > temperatureRange[1] + 10)
            {
                return false;
			} else {
				return true;
			}
        }


        // Return the growing crops Quality
        private double calculateCropQuality(int day, double rainSum, int[] dailyTemps)
        {
            int goodDays = 0;
            int startGrowthDay = day - growthPeriod;
            // temperature multiplier is % of days that are within the ideal temperature range.
            for (int d = day; d > startGrowthDay; d--)
            {
				if (dailyTemps[d] >= temperatureRange[0] || dailyTemps[d] <= temperatureRange[1])
                {
                    goodDays++;
                }
            }
            // rain multiplier is between 50% and 125%  based on how close to ideal the rainfall level was.
            double maxDist = (rainfallRange[1] - rainfallRange[0]) / 2.0;
			double idealRain = (rainfallRange[1] + rainfallRange[0]) / 2.0;
            double rainMultiplier = 1.25 - ((Math.Abs(rainSum - idealRain) / maxDist) * .75);
            // return the two modifiers used together.
			return (goodDays / growthPeriod) * rainMultiplier * 100.0;
        }


        // Determine if starting on today the previous days have a suitable rainfall sum.
		private bool dayRainAllowCrop(int day, double[] dailyRain, double[] dailyVolume, out double rainSum, out double percentGrowable)
        {
            // can grow ONLY if the rainfall is within the ideal rainfall range
			rainSum = 0.0;
			percentGrowable = 1.0;
			int startGrowthDay = day - growthPeriod;
			if (startGrowthDay > 0){
				double sum = 0.0;
                double surfaceSum = 0.0;
                // Sum the rainfall in the crops growing period
                for (int d = day; d > startGrowthDay; d--)
                {
                    sum += dailyRain[d];
				    surfaceSum += dailyVolume[d] * Habitats.RIVER_WATERING_MULTIPLIER;
                }
                // If that sum is in the acceptable range set the rainSum variable and return true, else return false.
                // Ideally if any value in the range of values from sum to sum + surfacewaterSum is between minWater and maxWater
                // Return true and rainSum set and percentGrowable set.
                // Else return false
                if ((sum > rainfallRange[0] && sum < rainfallRange[1]) || (sum < rainfallRange[0] && (sum + surfaceSum) > rainfallRange[1]))
                {
                    // Set the rainSum with the midpoint of the trapezoid of possible return values.
					rainSum = (Math.Min(sum + surfaceSum, rainfallRange[1]) + Math.Max(sum, rainfallRange[0])) / 2.0;
                    // Set the percent Growable as the percent of the tile that actually is in the acceptable water range.
                    if (surfaceSum.Equals(0.0))
                    {
                        percentGrowable = 1.0;
                    }
                    else
                    {
                        percentGrowable = (Math.Min(sum + surfaceSum, rainfallRange[1]) - Math.Max(sum, rainfallRange[0])) / surfaceSum;
                    }
                    // return true because stuff grows here
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
