using System.Collections;
using Prototype01;
using UnityEngine;

namespace Prototype02.GameStates
{
public class StartState : State
    {
        private static readonly Vector3 StartTileHighlightOffset = Vector3.up * (6.653846f - 6.538462f);
        private static readonly Vector3 StartTileHighlightStartPos = new Vector3(-2.0f, -1.34615469f, 0f);
        
        private TileMapper _tileMapper;
        private PlayerController _player;
        private TileHighlight _startTileHighlight;
        private GameController _gameController;

        public StartState(GameController gameController)
        {
            _gameController = gameController;
            _tileMapper = gameController.TileMapper;
            _player = gameController.Player;
            _player.ResetInternals(new Vector3(-2, -1.346154f, 0));
            gameController.Enemy.ResetInternals(new Vector3(2, 1.730769f, 0));
        }

        private void InitStartTile()
        {
            _startTileHighlight = _tileMapper.GetGameObject(_player.transform.position).GetComponent<TileHighlight>();
            _startTileHighlight.transform.position = StartTileHighlightStartPos;
            _startTileHighlight.gameObject.SetActive(true);
        }
        
        public override IEnumerator Init()
        {
            // Wait for start to be executed.
            yield return null;
            InitStartTile();

            var startColor = _startTileHighlight.SpriteRenderer.color;
            var t = 0.0f;
            var step = 0.01f;
            while (!_player.PlayerTileSelected)
            {
                if (t > 1.0f)
                {
                    step = -0.01f;
                }
                else if (t <= 0)
                {
                    step = 0.01f;
                }
                _startTileHighlight.SpriteRenderer.color = Color.Lerp(startColor, Color.white, t);
                t += step;
                yield return null;
            }
            _startTileHighlight.gameObject.SetActive(false);
            _gameController.SetState(new PlayerTurnState(_gameController));
        }
    }
}