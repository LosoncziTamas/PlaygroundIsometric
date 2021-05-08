using System.Collections.Generic;
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

            foreach (var node in _closedNodes)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(node.WorldPos,Vector3.one * 0.15f);
            }
            
            foreach (var node in _openNodes)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(node.WorldPos,Vector3.one * 0.15f);
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
                    var walkableTile = _tileMapper.WorldPosToTile(nextPos)?.GetType() != typeof(Obsctale);
                    if (walkableTile)
                    {
                        transform.position = nextPos;
                    }
                }
            }
        }

        private List<Node> _closedNodes = new List<Node>();
        private List<Node> _openNodes = new List<Node>();

        private int counter = 0;

        public void FindPath(Vector3 start, Vector3 end)
        {
            counter = 0;
            var openNodes = new List<Node>();
            var closedNodes = new HashSet<Node>();

            var startCell = _tileMapper.WorldPosToCell(start);
            var endCell = _tileMapper.WorldPosToCell(end);

            if (!startCell.HasValue || !endCell.HasValue)
            {
                return;
            }
            
            var startNode = new Node(startCell.Value, start, startCell.Value, endCell.Value);
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                var currNode = openNodes[0];
                for (var nodeIndex = 1; nodeIndex < openNodes.Count; ++nodeIndex)
                {
                    var otherNode = openNodes[nodeIndex];
                    var betterNode = otherNode.FCost < currNode.FCost ||
                                     Mathf.Approximately(otherNode.FCost, currNode.FCost) &&
                                     otherNode.HCost < currNode.HCost;
                    if (betterNode)
                    {
                        currNode = otherNode;
                    }
                }

                openNodes.Remove(currNode);
                closedNodes.Add(currNode);
                
                if (currNode.OnSameCell(endCell.Value))
                {
                    //path found
                    return;
                }

                counter++;
                if (counter > 10000)
                {
                    return;
                }
                
                var neighbourCells = _tileMapper.GetNeighbourCells(currNode.Cell);
                var neighbourNodes = new List<Node>();
                foreach (var neighbourCell in neighbourCells)
                {
                    var neighbourNode = new Node(neighbourCell, _tileMapper.CellToWorldPos(neighbourCell).GetValueOrDefault(), startCell.Value, endCell.Value);
                    if (closedNodes.Contains(neighbourNode))
                    {
                        continue;
                    }

                    var movementCostToNeighbour = currNode.GCost + Node.CellDistance(currNode.Cell, neighbourNode.Cell);
                    if (movementCostToNeighbour < neighbourNode.GCost || !openNodes.Contains(neighbourNode))
                    {
                        // TODO: update costs
                    }
                }
                
                
                _openNodes.Clear();
                _openNodes.AddRange(openNodes);
                
                _closedNodes.Clear();
                _closedNodes.AddRange(closedNodes);
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
