using System;
using System.Linq;
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
            var tiles = _tilemap.GetTilesBlock(_tilemap.cellBounds);
            var count = tiles.Length;
            var first = tiles.First(t => t != null);
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            var mousePos = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
            var worldPoint =  _camera.ScreenToWorldPoint(mousePos);
            var cellPos = _tilemap.WorldToCell(worldPoint);
            cellPos = new Vector3Int(cellPos.x, cellPos.y, 0);
            if (_tilemap.HasTile(cellPos))
            {
                _destination = mousePos;
            }
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

        private void OnEnable()
        {
            _mouseInput.Enable();
        }

        private void OnDisable()
        {
            _mouseInput.Disable();
        }
    }
}
