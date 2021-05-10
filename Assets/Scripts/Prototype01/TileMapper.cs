using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prototype01
{
    public class TileMapper : MonoBehaviour
    {
        [Serializable]
        public struct TileMapProps
        {
            public Tilemap Tilemap;
            public Vector3 Offset;
        }
        
        [SerializeField] private TileMapProps[] _tileMapPropses;
        
        private MouseInput _mouseInput;
        private Camera _camera;
        
        public Vector3Int? MouseHoveredCell {get; private set; }
        
        public Vector3? MouseHoveredTileWorldPos { get; private set; }

        private void Awake()
        {
            _mouseInput = new MouseInput();
            _camera = Camera.main;
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

            MouseHoveredCell = null;
            MouseHoveredTileWorldPos = null;
            
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var tile = tileMap.WorldToCell(screenPos);
                
                if (tileMap.HasTile(tile))
                {
                    MouseHoveredCell = tile;
                    MouseHoveredTileWorldPos = tileMap.CellToWorld(tile) + _tileMapPropses[i].Offset;
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
                        var obstacle = tile.GetType() == typeof(Obsctale);
                        var worldPos = tileMap.CellToWorld(relativePos);
                        Gizmos.color = obstacle ? Color.red : Color.green;
                        Gizmos.DrawCube(worldPos + offset, Vector3.one * 0.1f);
                        // Handles.Label(worldPos, relativePos.ToString());
                    }
                }
            }
        }
        
        [CanBeNull]
        public TileBase WorldPosToTile(Vector3 worldPos)
        {
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var cell = tileMap.WorldToCell(worldPos);
                if (tileMap.HasTile(cell))
                {
                    var tile = tileMap.GetTile(cell);
                    return tile;
                }
            }

            return null;
        }
        
        [CanBeNull]
        public T WorldPosToTile<T>(Vector3 worldPos) where T : TileBase
        {
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var cell = tileMap.WorldToCell(worldPos);
                if (tileMap.HasTile(cell))
                {
                    var tile = tileMap.GetTile(cell);
                    if (tile is T typedTile)
                    {
                        return typedTile;
                    }
                }
            }
            
            return default;
        }

        public List<Vector3Int> GetNeighbourCells(Vector3Int cell)
        {
            var result = new List<Vector3Int>();
            
            var cellBounds = new BoundsInt(cell.x - 1, cell.y - 1, cell.z - 1, 3, 3, 3);
            
            foreach (var boundInt in cellBounds.allPositionsWithin)
            {
                var relativePos = new Vector3Int(boundInt.x, boundInt.y, boundInt.z);
                for (var i = 0; i < _tileMapPropses.Length; i++)
                {
                    var tileMap = _tileMapPropses[i].Tilemap;
                    if (tileMap.HasTile(relativePos) && !relativePos.Equals(cell))
                    {
                        var tile = tileMap.GetTile(cell);
                        if (tile != null && tile.GetType() != typeof(Obsctale))
                        {
                            result.Add(relativePos);
                        }
                        else
                        {
                            // TODO: why elevated returns null?
                            Debug.Log("Tile was null/obstacle at " + relativePos);
                        }
                    }
                }
            }
            return result;
        }
        
        public Vector3Int? WorldPosToCell(Vector3 worldPos)
        {
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var cell = tileMap.WorldToCell(worldPos);
                if (tileMap.HasTile(cell))
                {
                    return cell;
                }
            }
            return null;
        }
        
        public Vector3? CellToWorldPos(Vector3Int cell)
        {
            for (var i = 0; i < _tileMapPropses.Length; i++)
            {
                var tileMap = _tileMapPropses[i].Tilemap;
                var offset = _tileMapPropses[i].Offset;
                if (tileMap.HasTile(cell))
                {
                    return tileMap.CellToWorld(cell) + offset;
                }
            }

            return null;
        }

    }
}