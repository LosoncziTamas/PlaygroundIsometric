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

        public void HighLight(SelectedTile tile)
        {
            Debug.Log("[ClickableTileBehaviour] HighLight");
        }
    }
}