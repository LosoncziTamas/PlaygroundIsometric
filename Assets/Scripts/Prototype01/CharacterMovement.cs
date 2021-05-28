using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype01
{
    public class CharacterMovement : MonoBehaviour
    {
        // TODO: bypass obstacle edges
        // TODO: fix movement between levels

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
                var path = FindPath(transform.position, tilePos.Value);
                StopAllCoroutines();
                StartCoroutine(MoveAlongPath(path));
                _path.Clear();
                _path.AddRange(path);
            }
        }
        
        private IEnumerator MoveAlongPath(IList<Node> path)
        {
            var lastNode = path.LastOrDefault();
            if (lastNode != null)
            {
                var lastCellWorldPos = _tileMapper.CellToWorldPos(lastNode.Cell).GetValueOrDefault();
                while (Vector3.Distance(lastCellWorldPos, transform.position) > 0.1)
                {
                    var next = path.FirstOrDefault();
                    if (next != null)
                    {
                        path.RemoveAt(0);
                        var worldPos = _tileMapper.CellToWorldPos(next.Cell).GetValueOrDefault();
                        while (Vector3.Distance(worldPos, transform.position) > 0.1)
                        {
                            var nextPos = Vector3.MoveTowards(transform.position, worldPos, Time.fixedDeltaTime * _speed);
                            Debug.Log($"[CharacterMovement] MoveToPosition target {worldPos} currPos {transform.position} nextPos {nextPos}");
                            transform.position = nextPos;
                            yield return new WaitForFixedUpdate();
                        }
                    }
                }
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

        private IList<Node> FindPath(Vector3 start, Vector3 end)
        {
            var openNodes = new Heap<Node>(_tileMapper.TotalCellCount);
            var closedNodes = new HashSet<Node>();

            var startCell = _tileMapper.WorldPosToCell(start);
            var endCell = _tileMapper.WorldPosToCell(end);

            if (!startCell.HasValue || !endCell.HasValue)
            {
                return Array.Empty<Node>();
            }
            
            var startNode = new Node(startCell.Value, start, startCell.Value, endCell.Value);
            openNodes.AddItem(startNode);

            while (openNodes.Count > 0)
            {
                var currNode = openNodes.RemoveFirst();
                closedNodes.Add(currNode);
                
                if (currNode.OnSameCell(endCell.Value))
                {
                    return RetracePath(startNode, currNode);
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

            return Array.Empty<Node>();
        }

        private static List<Node> RetracePath(Node start, Node end)
        {
            var path = new List<Node>();
            var curr = end;
            path.Add(curr);
            while (curr != null && !Equals(curr.Parent, start))
            {
                curr = curr.Parent;
                path.Add(curr);
            }
            path.Reverse();
            return path;
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
