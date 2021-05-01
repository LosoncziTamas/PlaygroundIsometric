using System.Collections.Generic;
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

            foreach (var cell in _neighbouringCells)
            {
                var worldPos =  MouseToTile.Instance.CellToWorldPos(cell);
                if (worldPos.HasValue)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(worldPos.Value,Vector3.one * 0.15f);
                }
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

        private List<Vector3Int> _neighbouringCells = new List<Vector3Int>();

        public void FindPath(Vector3 start, Vector3 end)
        {
            var openNodes = new List<Node>();
            var closedNodes = new List<Node>();

            var startCell = MouseToTile.Instance.WorldPosToCell(start);
            var endCell = MouseToTile.Instance.WorldPosToCell(end);

            if (!startCell.HasValue || !endCell.HasValue)
            {
                return;
            }
            
            var startNode = new Node(startCell.Value, start, start, end);
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                var currNode = openNodes[0];
                for (var nodeIndex = 1; nodeIndex < openNodes.Count; ++nodeIndex)
                {
                    if (currNode.FCost > openNodes[nodeIndex].FCost)
                    {
                        currNode = openNodes[nodeIndex];
                    }
                }

                openNodes.Remove(currNode);
                closedNodes.Add(currNode);

                if (currNode.OnSameCell(endCell.Value))
                {
                    //path found
                    return;
                }
                
                _neighbouringCells.Clear();
                var neighbourCells = MouseToTile.Instance.GetNeighbourCells(currNode.Cell);
                _neighbouringCells.AddRange(neighbourCells);
                return;
            }
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
