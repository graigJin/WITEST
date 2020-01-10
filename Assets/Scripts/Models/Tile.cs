using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    public class Tile
    {
        public enum TileType
        {
            LightGrass, Grass, DarkGrass, Empty
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
            switch (Random.Range(0, 3))
            {
                case 0:
                    Type = TileType.LightGrass;
                    break;
                case 1:
                    Type = TileType.Grass;
                    break;
                case 2:
                    Type = TileType.DarkGrass;
                    break;
                default:
                    Type = TileType.Empty;
                    break;
            }
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
