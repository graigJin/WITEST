using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Controllers
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Sprite floorSprite;
        
        [SerializeField] private Material emptyMaterial;
        [SerializeField] private Material floorMaterial;
        [SerializeField] private Material selectionMaterial;

        public World World { get; private set; }
        private Dictionary<Tile, GameObject> _tileGameObjectsDictionary;

        private Tile _currentSelection;
        private Material _currentMaterial;

        private UIController _ui;
        
        void Awake()
        {
            Instance = this;
            _ui = GameObject.FindWithTag("UI").GetComponent<UIController>();
            
            _tileGameObjectsDictionary = new Dictionary<Tile, GameObject>();
            World = new World(width, height);
            GenerateTileGameObjects();
        }

        void GenerateTileGameObjects()
        {
            foreach (Tile tile in World.Tiles)
            {
                tile.RegisterTileTypeChangeCallback(OnTileTypeChanged);
                
                GameObject go = new GameObject(tile.GetName());
                _tileGameObjectsDictionary.Add(tile, go);
                
                go.transform.position = new Vector3(tile.XPos, tile.YPos);
                go.transform.parent = transform;

                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = floorSprite;

                tile.RandomizeTileType();
            }
        }

        public void OnTileSelected(int x, int y)
        {
            Tile t = World.GetTileAt(x, y);
            
            // Check if currently another tile is selected
            if (_currentSelection != null)    
            {
                // Reset the currently selected tile's material to it's original material
                ApplyMaterialToCurrentSelection(_currentMaterial);
            }
            
            // Check if a tile has been clicked
            if (t != null)    
            {
                // Currently no other tile is selected
                if (_currentSelection == null)
                {
                    // Re-Activate the UI Panel
                    _ui.Panel.SetActive(true);
                }

                // Currently selected tile clicked again
                if (_currentSelection == t)
                {
                    // Reset the currently selected tile's material to it's original material
                    ApplyMaterialToCurrentSelection(_currentMaterial);
                    Deselect();
                    return;
                }

                // Set currently selected tile and material
                _currentSelection = t;
                _currentMaterial = GetMaterial(t);
                
                // Get GameObject and apply selectionMaterial
                ApplyMaterialToCurrentSelection(selectionMaterial);
                
                // Update UI to display currently selected tile
                _ui.ChangeMaterialSelection(t, GetMaterial(_currentSelection));
            }
            
            // No tile has been clicked
            else
            {
                Deselect();
            }
        }

        void ApplyMaterialToCurrentSelection(Material material)
        {
            _tileGameObjectsDictionary[_currentSelection].GetComponent<SpriteRenderer>().material = material;
        }

        void Deselect()
        {
            // Reset currently selected tile and material
            _currentSelection = null;
            _currentMaterial = null;
                
            // Deactivate the UI Panel
            _ui.Panel.SetActive(false);
        }

        void OnTileTypeChanged(Tile tile)
        {
            GameObject go = _tileGameObjectsDictionary[tile];
            
            switch (tile.Type)
            {
                case Tile.TileType.Floor:
                    go.GetComponent<SpriteRenderer>().material = floorMaterial;
                    break;
                default:
                    go.GetComponent<SpriteRenderer>().material = emptyMaterial;
                    break;
            }

            go.name = tile.GetName();
        }

        Material GetMaterial(Tile tile)
        {
            switch (tile.Type)
            {
                case Tile.TileType.Floor:
                    return floorMaterial;
                default:
                    return emptyMaterial;
            }
        }

    }
}
