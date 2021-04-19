using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prototype01
{
    public class MouseToTile : MonoBehaviour
    {
        private static readonly Vector3 TileOffset = Vector3.up * 0.5f; 
        
        public static MouseToTile Instance;
        
        [SerializeField] private Tilemap[] _tilemaps;
        
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
            
            for (var i = 0; i < _tilemaps.Length; i++)
            {
                var tileMap = _tilemaps[i];
                var tile = tileMap.WorldToCell(screenPos);
                
                if (tileMap.HasTile(tile))
                {
                    Tile = tile;
                    WorldPos = tileMap.CellToWorld(tile) + TileOffset;
                }
            }
        }
    }
}