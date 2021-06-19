using System.Collections.Generic;
using System.Linq;
using Prototype01;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype02
{
    public class SmartTileCursor : MonoBehaviour
    {
        private const int MaxSurroundingTileCount = 8;
        
        [SerializeField] private TileMapper _tileMapper;
        [SerializeField] private TileHighlight _tileHighlightPrefab;
        [SerializeField] private Transform _mainHighlight;

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
        
        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            var tilePos = _tileMapper.MouseHoveredTileWorldPos;
            if (tilePos.HasValue)
            {
                foreach (var displayedWalkable in _displayedWalkables)
                {
                    if (tilePos.Equals(displayedWalkable.transform.position))
                    {
                        Debug.Log("Walkable");
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

        public void HighlightWalkableTiles(Vector3 position)
        {
            MoveDisplayedToCached();
            
            var playerCell = _tileMapper.WorldPosToCell(position);
            if (playerCell.HasValue)
            {
                var neighbourCells = _tileMapper.GetNeighbourCells(playerCell.Value);
                foreach (var neighbourCell in neighbourCells)
                {
                    var pos = _tileMapper.CellToWorldPos(neighbourCell);
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
                    tileHighlight.Init(Color.green);
                }
            }
        }

        private void Update()
        {
            var tilePos = _tileMapper.MouseHoveredTileWorldPos;
            if (tilePos.HasValue)
            {
                _mainHighlight.position = tilePos.Value;
            }
        }
    }
}