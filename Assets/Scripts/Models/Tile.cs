using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    public class Tile
    {
        public enum TileType
        {
            LightGrass, Grass, DarkGrass, LightSand, Sand, DarkSand, Empty
        }

        public int XPos { get; }
        public int YPos { get; }

        public TileType Type
        {
            get => _type;
            set
            {
                _type = value;
                if (_cbTileTypeChange != null)
                    _cbTileTypeChange(this);
            }
        }

        private TileType _type;
        private Action<Tile> _cbTileTypeChange;

        public Tile(int xPos, int yPos)
        {
            XPos = xPos;
            YPos = yPos;
        }

        public void RandomizeTileType()
        {
            int r = Random.Range(0, 100);

            if (r < 25)
            {
                Type = TileType.LightGrass;
                return;
            }
            
            if (r < 50)
            {
                Type = TileType.Grass;
                return;
            }
            
            if (r < 75)
            {
                Type = TileType.DarkGrass;
                return;
            }
            
            if (r < 85)
            {
                Type = TileType.LightSand;
                return;
            }
            
            if (r < 95)
            {
                Type = TileType.Sand;
                return;
            }

            Type = TileType.DarkSand;
        }

        public void RegisterTileTypeChangeCallback(Action<Tile> callback)
        {
            _cbTileTypeChange += callback;
        }
        
        public String GetName()
        {
            return XPos + "|" + YPos + "|" + Type;
        }
    }
}
