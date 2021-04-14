using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Prototype01
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private float _speed = 2.0f;
        
        private MouseInput _mouseInput;
        private Camera _camera;
        private Vector3? _destination;

        private void Awake()
        {
            _mouseInput = new MouseInput();
            _camera = Camera.main;
        }
        
        private void Start()
        {
            _mouseInput.Mouse.MouseClick.performed += OnMouseClick;
        }
        
        private void OnEnable()
        {
            _mouseInput.Enable();
        }
        
        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            var mousePos = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
            var tile = GetTileFromMousePos(mousePos);
            if (tile.HasValue)
            {
                _destination = _tilemap.CellToWorld(tile.Value);
            }
        }

        private Vector3Int? GetTileFromMousePos(Vector2 mousePos)
        {
            var screenPos = _camera.ScreenToWorldPoint(mousePos);
            screenPos.z = 0;

            var tile = _tilemap.WorldToCell(screenPos);
            
            if (_tilemap.HasTile(tile))
            {
                return tile;
            }

            return null;
        }

        private void FixedUpdate()
        {
            if (_destination.HasValue)
            {
                var targetPos = _destination.Value;
                var currentPos = transform.position;
                if (Vector3.Distance(targetPos, currentPos) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(currentPos, targetPos, Time.deltaTime * _speed);
                }
            }
        }

        private Vector2 _lastMousePos = Vector3.zero;

        private void Update()
        {
            var currMousePos = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
            
            if (Vector2.Distance(currMousePos, _lastMousePos) > 0.01f)
            {
                var tile = GetTileFromMousePos(currMousePos);
                if (tile.HasValue)
                {
                    var hoveredTile = _tilemap.GetTile(tile.Value);
                    if (hoveredTile is ClickableTile clickableTile)
                    {
                        clickableTile.Highlight();
                    }
                }
            }

            _lastMousePos = currMousePos;
        }

        private void OnDisable()
        {
            _mouseInput.Disable();
        }

        private void OnDestroy()
        {
            _mouseInput.Mouse.MouseClick.performed -= OnMouseClick;
        }
    }
}
