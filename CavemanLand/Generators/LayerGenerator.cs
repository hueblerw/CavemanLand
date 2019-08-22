using System;

namespace CavemanLand.Generators
{
    public class LayerGenerator
    {
		private Random randy;
		private int X;
		private int Z;
		private int roundTo;

        public LayerGenerator(int X, int Z, int roundTo)
		{
			this.X = X;
			this.Z = Z;
			this.roundTo = roundTo;
			randy = new Random();
		}

		public double[,] GenerateWorldLayer(double min, double max, double maxChange, double startingValue, bool squared)
        {
            double[,] layer = new double[X, Z];
			layer[0, 0] = Math.Round(startingValue, roundTo);
            layer = BuildTopRow(layer, min, max, maxChange, squared);
            layer = BuildLeftMostColumn(layer, min, max, maxChange, squared);
			layer = FillOutRemainingWorld(layer, min, max, maxChange, squared);
            return layer;
        }

		public int[,] convertDoubleArrayToInt(double[,] array)
        {
            int[,] intArray = new int[X, Z];
            for (int x = 0; x < X; x++)
            {
                for (int z = 0; z < Z; z++)
                {
                    intArray[x, z] = (int)array[x, z];
                }
            }
            return intArray;
        }

		private double[,] FillOutRemainingWorld(double[,] layer, double min, double max, double maxChange, bool squared)
        {
            for (int i = 1; i < layer.GetLength(0); i++)
            {
                for (int j = 1; j < layer.GetLength(1); j++)
                {
                    double change = CalculateChange(randy.NextDouble(), maxChange, squared);
                    double average = (layer[i - 1, j] + layer[i, j - 1]) / 2.0;
					layer[i, j] = Math.Round(Math.Max(Math.Min(average + change, max), min), roundTo);
                }
            }
            return layer;
        }

        private double[,] BuildLeftMostColumn(double[,] layer, double min, double max, double maxChange, bool squared)
        {
            for (int i = 1; i < layer.GetLength(1); i++)
            {
                double change = CalculateChange(randy.NextDouble(), maxChange, squared);
				layer[0, i] = Math.Round(Math.Max(Math.Min(layer[0, i - 1] + change, max), min), roundTo);
            }
            return layer;
        }

		private double[,] BuildTopRow(double[,] layer, double min, double max, double maxChange, bool squared)
        {
            for (int i = 1; i < layer.GetLength(0); i++)
            {
                double change = CalculateChange(randy.NextDouble(), maxChange, squared);
				layer[i, 0] = Math.Round(Math.Max(Math.Min(layer[i - 1, 0] + change, max), min), roundTo);
            }
            return layer;
        }

		private double CalculateChange(double randomDouble, double maxChange, bool squared)
        {
            double change = randomDouble;
            if (squared)
            {
                change = Math.Pow(change, 2);
            }
            change = change * maxChange * randomSign();
            return change;
        }

		private int randomSign()
        {
            if (randy.NextDouble() > .5)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
