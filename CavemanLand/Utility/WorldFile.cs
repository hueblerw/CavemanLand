namespace CavemanLand.Utility
{
    public class WorldFile
    {
		public string name;
		public int[] dimensions;
		public WorldDate currentDate;
		public string tilesFilepath;
		public string herdsFilepath;
		public string tribesFilepath;

        public WorldFile()
        {
        }

        public WorldFile(string name, int[] dimensions, WorldDate currentDate, string tilesFilepath, string herdsFilepath, string tribesFilepath)
		{
			this.name = name;
			this.dimensions = dimensions;
			this.currentDate = currentDate;
			this.tilesFilepath = tilesFilepath;
			this.herdsFilepath = herdsFilepath;
			this.tribesFilepath = tribesFilepath;
		}
    }
}
