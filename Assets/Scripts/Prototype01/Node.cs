using System;
using UnityEngine;

namespace Prototype01
{
    public class Node
    {
        private const float UnitDistance = 10.0f;
        private static readonly float UnitDistanceDiagonal = Mathf.Sqrt(UnitDistance);
        
        public float FCost { get; }
        
        public float HCost { get; }

        public Vector3Int Cell { get; }
        
        public Vector3 WorldPos { get; }

        public Node(Vector3Int cellPos, Vector3 worldPos, Vector3 startPos, Vector3Int endCellPos)
        {
            Cell = cellPos;
            WorldPos = worldPos;
            HCost = CellDistance(endCellPos, cellPos);
            var gCost = Vector3.Distance(startPos, worldPos);
            FCost = gCost + HCost;
        }

        public static float CellDistance(Vector3Int start, Vector3Int end)
        {
            var distance = 0f;
            var dX = Mathf.Abs(start.x - end.x);
            var dY = Mathf.Abs(start.y - end.y);
            
            if (dX > dY)
            {
                distance = UnitDistanceDiagonal * dY + UnitDistance * (dX - dY);
            }
            else 
            {
                distance = UnitDistanceDiagonal * dX + UnitDistance * (dY - dX);
            }
            
            return distance;
        }

        public override string ToString()
        {
            return $"Cell: {Cell} FCost {FCost} HCost {HCost}";
        }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(Node))
            {
                return false;
            }
            return ((Node) other).Cell.Equals(Cell);
        }

        public override int GetHashCode()
        {
            return Cell.GetHashCode();
        }

        public bool OnSameCell(Vector3Int otherCell)
        {
            return Cell.Equals(otherCell);
        }
    }
}