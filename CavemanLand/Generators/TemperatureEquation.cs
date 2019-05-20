using System;

namespace CavemanLand.Generators
{
    public class TemperatureEquation
    {
        private double A;
        private double B;
        private double C;

        private double curveMin;
        private double variance;
        private double amplitude;
        private Random randy;

        public TemperatureEquation(int lowTemp, int highTemp, int summerLength, double variance)
        {
            A = (summerLength - 1) / ((-1.0 / 4.0) * summerLength * (Math.Pow(summerLength, 2) - 4.0));
            B = -3.0 * A;
            C = 1.0 - A - B;
            this.variance = variance;
            amplitude = (highTemp - lowTemp) / 2.0;
            curveMin = lowTemp + amplitude;
            randy = new Random();
        }

        public int getTodaysTemp(int day)
        {
            double x = (day - 1) * (Math.PI / 60.0);  // Do something to the day.  Should go from 0 to 2 PI, but days 1 to 120
            double G = A * Math.Pow(x, 3) + B * Math.Pow(x, 2) + C * x;
            double curveTemp = -amplitude * Math.Cos(Math.PI * G) + curveMin;
            double temp = variance * randy.NextDouble() - (variance / 2.0) + curveTemp;
            return (int)Math.Round(temp, 0);
        }

        public override string ToString()
        {
            return "-" + amplitude + "cos(PI * [" + A + "x^3 + " + B + "x^2 + " + C + "x]) + " + curveMin;
        }
    }
}
