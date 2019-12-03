using System;

namespace CavemanLand.Controllers
{
    public class Runner
    {
		public static void Main()
		{
			WorldController controller = new WorldController();
			controller.generateWorld(50, 50);
			controller.saveWorld("Wills World");
			controller.printTileInfo(10, 31);
			controller.printTileInfo(20, 21);
			controller.printTileInfo(21, 21);
			controller.printTileInfo(30, 11);
			controller.printTileInfo(40, 45);
			Console.WriteLine(controller.GetWorld().getWorldHabitatComposition());
		}
    }
}
