using Prototype01;
using UnityEngine;

namespace Prototype02
{
    public class SmartTileCursor : MonoBehaviour
    {
        [SerializeField] private Transform _mainHighlight;

        private void Update()
        {
            var tilePos = TileMapper.Instance.MouseHoveredTileWorldPos;
            if (tilePos.HasValue)
            {
                _mainHighlight.position = tilePos.Value;
            }
        }
    }
}