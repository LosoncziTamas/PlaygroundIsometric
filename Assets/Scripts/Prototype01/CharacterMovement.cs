using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype01
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 2.0f;
        
        private MouseInput _mouseInput;
        private Vector3? _destination;
        
        private void Awake()
        {
            _mouseInput = new MouseInput();
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
            var tilePos = MouseToTile.Instance.WorldPos;
            if (tilePos.HasValue)
            {
                _destination = tilePos.Value;
                FindPath(transform.position, tilePos.Value);
            }
        }

        private void OnDrawGizmos()
        {
            if (_destination.HasValue)
            {
                Gizmos.DrawLine(transform.position, _destination.Value);
            }
        }
        
        private void FixedUpdate()
        {
            if (_destination.HasValue)
            {
                var targetPos = _destination.Value;
                var currentPos = transform.position;
                if (Vector3.Distance(targetPos, currentPos) > 0.01f)
                {
                    var nextPos = Vector3.MoveTowards(currentPos, targetPos, Time.deltaTime * _speed);
                    if (MouseToTile.Instance.TileIsWalkable(nextPos))
                    {
                        transform.position = nextPos;
                    }
                }
            }
        }

        public void FindPath(Vector3 start, Vector3 end)
        {
            
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
