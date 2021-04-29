using UnityEngine;

namespace Prototype01
{
    public class Node
    {
        private readonly Vector3 _worldPos;
        
        public float FCost { get; }
        
        public float HCost { get; }

        public Vector3Int Cell { get; }

        public Node(Vector3Int cellPos, Vector3 worldPos, Vector3 startPos, Vector3 endPos)
        {
            Cell = cellPos;
            _worldPos = worldPos;
            HCost = Vector3.Distance(endPos, worldPos);
            var gCost = Vector3.Distance(startPos, worldPos);
            FCost = gCost + HCost;
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