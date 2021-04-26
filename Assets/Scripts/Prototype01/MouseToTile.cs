using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prototype01
{
    public class MouseToTile : MonoBehaviour
    {
        [Serializable]
        public struct TileMapProps
        {
            public Tilemap Tilemap;
            public Vector3 Offset;
        }
        
        public static MouseToTile Instance;
        
        [SerializeField] private TileMapProps[] _tileMapPropses;
        
        private MouseInput _mouseInput;
        private Camera _camera;
        
        public Vector3Int? Tile {get; private set; }
        
        public Vector3? WorldPos { get; private set; }

        private void Awake()
        {
            _mouseInput = new MouseInput();
            _camera = Camera.main;
            Instance = this;
        }
        
        private void OnEnable()
        {
            _mouseInput.Enable();
        }

        private void OnDisable()
        {
            _mouseInput.Disable();
        }

        private void Update()
        {
            var mouse = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
            var screenPos = _camera.ScreenToWorldPoint(mouse);
            screenPos.z = 0;

            Tile = null;
            WorldPos = null;
            
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var tile = tileMap.WorldToCell(screenPos);
                
                if (tileMap.HasTile(tile))
                {
                    Tile = tile;
                    WorldPos = tileMap.CellToWorld(tile) + _tileMapPropses[i].Offset;
                }
            }
        }

        private void OnDrawGizmos()
        {
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var offset = _tileMapPropses[i].Offset;
                foreach (var boundInt in tileMap.cellBounds.allPositionsWithin)
                {
                    var relativePos = new Vector3Int(boundInt.x, boundInt.y, boundInt.z);
                    if (tileMap.HasTile(relativePos))
                    {
                        var tile = tileMap.GetTile(relativePos);
                        var walkable = tile.GetType() == typeof(WalkableTile);
                        var worldPos = tileMap.CellToWorld(relativePos);
                        Gizmos.color = walkable ? Color.green : Color.red;
                        Gizmos.DrawCube(worldPos + offset, Vector3.one * 0.1f);
                    }
                }
            }
        }

        public bool TileIsWalkable(Vector3 worldPos)
        {
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var cell = tileMap.WorldToCell(worldPos);
                if (tileMap.HasTile(cell))
                {
                    return true;
                }
            }

            return false;
        }
    }
}