using System.IO;

namespace CavemanLand.Utility
{
	public class MyJsonFileInteractor
    {
		public static string loadJsonFileToString(string pathname)
        {
            return File.ReadAllText(pathname);
        }

		public static void writeFileToPath(string filename, string json)
        {
            File.WriteAllText(filename, json);
        }
    }
}
