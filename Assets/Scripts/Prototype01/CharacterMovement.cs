using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype01
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 2.0f;
        [SerializeField] private TileMapper _tileMapper;

        private MouseInput _mouseInput;
        private Vector3? _destination;
        
        private readonly List<Node> _path = new List<Node>();
        
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
            var tilePos = _tileMapper.MouseHoveredTileWorldPos;
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

            foreach (var node in _path)
            {
                var worldPos = _tileMapper.CellToWorldPos(node.Cell).Value;
                Handles.Label(worldPos, node.ToString());
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(worldPos,Vector3.one * 0.15f);
            }
        }

        public void FindPath(Vector3 start, Vector3 end)
        {
            var openNodes = new Heap<Node>(_tileMapper.TotalCellCount);
            var closedNodes = new HashSet<Node>();

            var startCell = _tileMapper.WorldPosToCell(start);
            var endCell = _tileMapper.WorldPosToCell(end);

            if (!startCell.HasValue || !endCell.HasValue)
            {
                return;
            }
            
            var startNode = new Node(startCell.Value, start, startCell.Value, endCell.Value);
            openNodes.AddItem(startNode);

            while (openNodes.Count > 0)
            {
                var currNode = openNodes.RemoveFirst();
                closedNodes.Add(currNode);
                
                if (currNode.OnSameCell(endCell.Value))
                {
                    RetracePath(startNode, currNode);
                    return;
                }
                
                var neighbourCells = _tileMapper.GetNeighbourCells(currNode.Cell);
                foreach (var neighbourCell in neighbourCells)
                {
                    var neighbourNode = new Node(neighbourCell, _tileMapper.CellToWorldPos(neighbourCell).GetValueOrDefault(), startCell.Value, endCell.Value);
                    if (closedNodes.Contains(neighbourNode))
                    {
                        continue;
                    }

                    var movementCostToNeighbour = currNode.GCost + Node.CellDistance(currNode.Cell, neighbourNode.Cell);
                    var foundMoreOptimalPath = movementCostToNeighbour < neighbourNode.GCost;
                    if (foundMoreOptimalPath)
                    {
                        Debug.Log("Found a more optimal way.");
                    }
                    if (foundMoreOptimalPath || !openNodes.Contains(neighbourNode))
                    {
                        neighbourNode.GCost = movementCostToNeighbour;
                        neighbourNode.Parent = currNode;

                        if (!openNodes.Contains(neighbourNode))
                        {
                            openNodes.AddItem(neighbourNode);
                        }
                        else
                        {
                            openNodes.UpdateItem(neighbourNode);
                        }
                    }
                }
            }
        }

        private void RetracePath(Node start, Node end)
        {
            var path = new List<Node>();
            var curr = end;
            while (!Equals(curr.Parent, start))
            {
                curr = curr.Parent;
                path.Add(curr);
            }
            
            path.Reverse();
            
            _path.Clear();
            _path.AddRange(path);
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
