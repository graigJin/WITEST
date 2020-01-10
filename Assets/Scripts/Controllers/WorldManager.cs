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
        
        [SerializeField] private Material lightGrassMaterial;
        [SerializeField] private Material grassMaterial;
        [SerializeField] private Material darkGrassMaterial;
        
        [SerializeField] private Material lightSandMaterial;
        [SerializeField] private Material SandMaterial;
        [SerializeField] private Material darkSandMaterial;

        public World World { get; private set; }
        private Dictionary<Tile, GameObject> _tileGameObjectsDictionary;
        private Dictionary<Tile.TileType, Material> _tileTypeMaterialDictionary;

        private Tile _currentSelection;
        private Material _currentMaterial;

        private UIController _ui;
        
        void Awake()
        {
            Instance = this;
            _ui = GameObject.FindWithTag("UI").GetComponent<UIController>();
            
            _tileGameObjectsDictionary = new Dictionary<Tile, GameObject>();
            SetUpTileTypeDictionary();
            
            World = new World(width, height);
            GenerateTileGameObjects();
        }

        void SetUpTileTypeDictionary()
        {
            _tileTypeMaterialDictionary = new Dictionary<Tile.TileType, Material>();
            
            _tileTypeMaterialDictionary.Add(Tile.TileType.Empty, emptyMaterial);
            _tileTypeMaterialDictionary.Add(Tile.TileType.Grass, grassMaterial);
            _tileTypeMaterialDictionary.Add(Tile.TileType.LightGrass, lightGrassMaterial);
            _tileTypeMaterialDictionary.Add(Tile.TileType.DarkGrass, darkGrassMaterial);
            _tileTypeMaterialDictionary.Add(Tile.TileType.LightSand, lightSandMaterial);
            _tileTypeMaterialDictionary.Add(Tile.TileType.Sand, SandMaterial);
            _tileTypeMaterialDictionary.Add(Tile.TileType.DarkSand, darkSandMaterial);
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
            go.GetComponent<SpriteRenderer>().material = _tileTypeMaterialDictionary[tile.Type];
            go.name = tile.GetName();
        }

        Material GetMaterial(Tile tile)
        {
            return _tileTypeMaterialDictionary[tile.Type];
        }

    }
}
