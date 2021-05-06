using UnityEngine;

namespace Prototype01
{
    public class TileCursor : MonoBehaviour
    {
        [SerializeField] private TileMapper _tileMapper;
        
        private void Update()
        {
            var tilePos = _tileMapper.MouseHoveredTileWorldPos;
            if (tilePos.HasValue)
            {
                transform.position = tilePos.Value;
            }
        }
    }
}