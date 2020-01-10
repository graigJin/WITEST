using UnityEngine;

namespace Models
{
    public class World
    {
        public int Width { get; }
        public int Height { get; }

        public Tile[,] Tiles { get; }

        public World(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[width,height];
            GenerateTileData();
        }

        void GenerateTileData()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tiles[x,y] = new Tile(x,y);
                }
            }
        }

        public Tile GetTileAt(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Tiles[x,y];                
            }

            return null;
        }
    }
}
