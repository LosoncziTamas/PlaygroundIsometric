using UnityEngine;

namespace Prototype01
{
    public class TileCursor : MonoBehaviour
    {
        private void Update()
        {
            var tilePos = MouseToTile.Instance.WorldPos;
            if (tilePos.HasValue)
            {
                transform.position = tilePos.Value;
            }
        }
    }
}