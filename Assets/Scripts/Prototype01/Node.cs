using UnityEngine;

namespace Prototype01
{
    public class Node
    {
        private readonly Vector3Int _cellPos;
        private readonly Vector3 _worldPos;
        
        public float FCost { get; }
        
        public float HCost { get; }

        public Node(Vector3Int cellPos, Vector3 worldPos, Vector3 startPos, Vector3 endPos)
        {
            _cellPos = cellPos;
            _worldPos = worldPos;
            HCost = Vector3.Distance(endPos, worldPos);
            var gCost = Vector3.Distance(startPos, worldPos);
            FCost = gCost + HCost;
        }

        public override string ToString()
        {
            return $"Cell: {_cellPos} FCost {FCost} HCost {HCost}";
        }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(Node))
            {
                return false;
            }
            return ((Node) other)._cellPos.Equals(_cellPos);
        }

        public override int GetHashCode()
        {
            return _cellPos.GetHashCode();
        }

        public bool OnSameCell(Vector3Int otherCell)
        {
            return _cellPos.Equals(otherCell);
        }
    }
}