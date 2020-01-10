using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models
{
    public class Tile
    {
        public enum TileType
        {
            Empty, Floor
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
            switch (Random.Range(0, 2))
            {
                case 0:
                    Type = TileType.Empty;
                    break;
                case 1:
                    Type = TileType.Floor;
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

        public void ChangeTileType()
        {
            if (Type == TileType.Empty)
            {
                Type = TileType.Floor;
            }
            else
            {
                Type = TileType.Empty;
            }
        }

        public String GetName()
        {
            return XPos + "|" + YPos + "|" + Type;
        }
    }
}
