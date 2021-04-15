using System;
using UnityEngine;

namespace Prototype01
{
    public class ClickableTileBehaviour : MonoBehaviour
    {
        public Color highlightColor = Color.yellow;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            
        }

        public void HighLight(ClickableTile tile)
        {
            tile.color = highlightColor;
            Debug.Log("[ClickableTileBehaviour] HighLight");
            _spriteRenderer.color = highlightColor;
        }
    }
}