namespace ArcaneNebula
{
    public struct LevelData
    {
        public int ID;
        public string Name;
        public string Data;

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}";
        }
    }

    public class Level
    {
        public int ID = -1;
        public string Name;
        public TileData[] TilesData;
    }
}
