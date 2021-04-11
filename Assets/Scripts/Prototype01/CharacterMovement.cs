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

        private Vector3Int _tile;
        
        private void Start()
        {
            _mouseInput.Mouse.MouseClick.performed += OnMouseClick;
            var tiles = _tilemap.GetTilesBlock(_tilemap.cellBounds);
            
            foreach (var position in _tilemap.cellBounds.allPositionsWithin) 
            {
                if (_tilemap.HasTile(position))
                {
                    _tile = position;
                    Debug.Log("default pos" + position);
                    return;
                }
            }
        }
        
        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            var mousePos = _mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(mousePos);
        
            if (Physics.Raycast(ray, out hit)) 
            {
                Transform objectHit = hit.transform;
                Debug.Log(objectHit.name);
            }
            
            if (_tilemap.HasTile(_tile))
            {
                _destination = _tilemap.CellToWorld(_tile);
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
