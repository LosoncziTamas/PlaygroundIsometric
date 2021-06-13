using UnityEngine;

namespace Prototype02
{
    public class TileHighlight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void Init(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}