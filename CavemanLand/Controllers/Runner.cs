using System;

namespace CavemanLand.Controllers
{
    public class Runner
    {
		public static void Main()
		{
			WorldController controller = new WorldController();
			controller.loadGeneralFiles();
			controller.generateWorld();
		}
    }
}
