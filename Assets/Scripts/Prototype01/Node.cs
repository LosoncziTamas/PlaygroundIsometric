using UnityEngine;

namespace Prototype01
{
    public class Node : IHeapItem<Node>
    {
        private const float UnitDistance = 10.0f;
        private static readonly float UnitDistanceDiagonal =  UnitDistance * Mathf.Sqrt(2);

        public float FCost => GCost + HCost;
        
        public float HCost { get; set; }
        
        public float GCost { get; set; }

        public Vector3Int Cell { get; }
        
        public Vector3 WorldPos { get; }
        
        public Node Parent { get; set; }

        public Node(Vector3Int cellPos, Vector3 worldPos, Vector3Int startCellPos, Vector3Int endCellPos)
        {
            Cell = cellPos;
            WorldPos = worldPos;
            HCost = CellDistance(endCellPos, cellPos);
            GCost = CellDistance(startCellPos, cellPos);
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

        public int CompareTo(Node other)
        {
            var result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = HCost.CompareTo(other.HCost);
            }
            
            // Negate result because the lower the cost the more optimal the way is.
            return -result;
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

        public int HeapIndex { get; set; }
    }
}