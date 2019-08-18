using System;
using CavemanLand.Utility;

namespace CavemanLand.Generators
{
    public class TemperatureEquation
    {      
        private double A;
        private double B;

        private double curveMin;
        private double avgTemp;
        private double variance;
        private Random randy;
        
        public TemperatureEquation(int lowTemp, int highTemp, int summerLength, double variance)
        {
            this.randy = new Random();
            this.variance = variance;
            this.avgTemp = (highTemp - lowTemp) / 2.0;
            curveMin = (double) lowTemp + avgTemp;
            int daysInYear = WorldDate.DAYS_PER_YEAR;
            double halfSummerLength = summerLength / 2.0;
            solveQuadratic(daysInYear, halfSummerLength);
        }

        public int getTodaysTemp(int day)
        {
            double randFactor = randy.NextDouble() * variance - (variance / 2.0);
            return (int) Math.Round(randFactor + baseTemp(day), 0);
        }

        public override string ToString()
        {
            return "-" + avgTemp + "cos(" + A + "x^2 + " + B + "x) + " + curveMin;
        }
        
        private void solveQuadratic(int daysInYear, double halfSummerLength)
        {
            A = (Math.PI * (halfSummerLength - 30.0)) / (60.0 * (halfSummerLength - 60.0) * halfSummerLength);
            B = (Math.PI * (Math.Pow(halfSummerLength, 2) - 120.0 * halfSummerLength + 1800.0)) / (60.0 * (halfSummerLength - 60.0) * halfSummerLength);
        }
        
        private double baseTemp(int day){
            if (day > 60)
            {
                return equationTemp(120 - day);
            }
            else
            {
                return equationTemp(day);
            }
        }
        
        private double equationTemp(int day)
        {
            return -avgTemp * Math.Cos(A * Math.Pow(day, 2) + B * day) + curveMin;
        }
    }
}
