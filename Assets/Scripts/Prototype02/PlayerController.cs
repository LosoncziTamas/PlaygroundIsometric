using System;
using System.Collections.Generic;
using System.Linq;
using Prototype01;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype02
{
    public class PlayerController : MonoBehaviour
    {
        private const int MaxSurroundingTileCount = 8;

        public Color WalkableColor;
        public event Action playerMoved;

        [SerializeField] private TileHighlight _tileHighlightPrefab;

        private readonly List<TileHighlight> _cachedWalkables = new List<TileHighlight>(MaxSurroundingTileCount);
        private readonly List<TileHighlight> _displayedWalkables = new List<TileHighlight>(MaxSurroundingTileCount);
        
        private MouseInput _mouseInput;
        
        private void Awake()
        {
            for (var i = 0; i < MaxSurroundingTileCount; i++)
            {
                var highlight = Instantiate(_tileHighlightPrefab, transform);
                highlight.gameObject.SetActive(false);
                _cachedWalkables.Add(highlight);
            }
            _mouseInput = new MouseInput();
        }

        private void Start()
        {
            _mouseInput.Mouse.MouseClick.performed += OnMouseClick;
        }
        
        private void OnEnable()
        {
            _mouseInput.Enable();
        }
        
        private void OnDisable()
        {
            _mouseInput.Disable();
        }

        private void OnDestroy()
        {
            _mouseInput.Mouse.MouseClick.performed -= OnMouseClick;
        }
        
        public void ResetInternals(Vector3 startPos)
        {
            transform.position = startPos;
            HideWalkableTiles();
        }
        
        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            var tilePos = TileMapper.Instance.MouseHoveredTileWorldPos;
            if (tilePos.HasValue)
            {
                foreach (var displayedWalkable in _displayedWalkables)
                {
                    if (tilePos.Equals(displayedWalkable.transform.position))
                    {
                        HideWalkableTiles();
                        this.StartMoveToPosition(displayedWalkable.transform.position, 2.0f, playerMoved);
                        return;
                    }
                }
            }
        }

        private void MoveDisplayedToCached()
        {
            foreach (var displayedWalkable in _displayedWalkables)
            {
                displayedWalkable.gameObject.SetActive(false);
            }
            _cachedWalkables.AddRange(_displayedWalkables);
            _displayedWalkables.Clear();
        }

        private void HighlightWalkableTiles(Vector3 position)
        {
            MoveDisplayedToCached();
            
            var playerCell = TileMapper.Instance.WorldPosToCell(position);
            if (playerCell.HasValue)
            {
                var neighbourCells = TileMapper.Instance.GetNeighbourCells(playerCell.Value);
                foreach (var neighbourCell in neighbourCells)
                {
                    var pos = TileMapper.Instance.CellToWorldPos(neighbourCell);
                    if (pos == null)
                    {
                        return;
                    }
                    
                    var tileHighlight = _cachedWalkables.FirstOrDefault();
                    if (tileHighlight == null)
                    {
                        return;
                    }
                    _cachedWalkables.Remove(tileHighlight);
                    _displayedWalkables.Add(tileHighlight);

                    tileHighlight.transform.position = pos.Value;
                    tileHighlight.gameObject.SetActive(true);
                    tileHighlight.Init(WalkableColor);
                }
            }
        }

        private void HideWalkableTiles()
        {
            MoveDisplayedToCached();
        }
        
        public void BeginTurn()
        {
            HighlightWalkableTiles(transform.position);
        }
    }
}