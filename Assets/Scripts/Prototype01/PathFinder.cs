using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Prototype01
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] private TileMapper _tileMapper;
        
        public Task<IList<Node>> FindPathAsync(Vector3 start, Vector3 end)
        {
            var result = Task.Run(() => FindPath(start, end));
            return result;
        }

        public IList<Node> FindPath(Vector3 start, Vector3 end)
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
            while (curr.Parent != null && !Equals(curr.Parent, start))
            {
                curr = curr.Parent;
                path.Add(curr);
            }
            path.Reverse();
            return path;
        }
    }
}