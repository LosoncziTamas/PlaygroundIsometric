using System;
using UnityEngine;

namespace Prototype02
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SmartTileCursor _smartTileCursor;

        public void BeginTurn()
        {
            _smartTileCursor.HighlightWalkableTiles(transform.position);
            
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Begin Player turn"))
            {
                BeginTurn();
            }
        }
    }
}