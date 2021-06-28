using System.Collections;
using Prototype01;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Prototype02
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private EnemyController _enemy;

        private bool _running;
        private bool _playersTurn;

        private Tile _startTile;
        private TileHighlight _startTileHighlight;

        enum State
        {
            PlayerStart,
            PlayerTurn,
            EnemyTurn,
            GameOver
        };

        private State _gameState;
        
        private void Start()
        {
            _startTile = TileMapper.Instance.WorldPosToTile<StartTile>(_player.transform.position);

            _player.playerMoved += OnPlayerMoved;
            _enemy.enemyMoved += OnEnemyMoved;
            _gameState = State.PlayerStart;
        }

        private IEnumerator PlayerStart()
        {
            var startColor = _startTileHighlight.SpriteRenderer.color;
            var t = 0.0f;
            while (!_player.PlayerTileSelected)
            {
                if (t > 1.0f)
                {
                    t = 0;
                }
                _startTileHighlight.SpriteRenderer.color = Color.Lerp(Color.green, Color.white, t);
                t += 0.01f;
                yield return null;
            }
        }
        
        private void OnGUI()
        {
            if (!_running && GUILayout.Button("Start Game"))
            {
                var go = TileMapper.Instance.GetGameObject(_player.transform.position);
                _startTileHighlight = go.GetComponent<TileHighlight>();
                StartCoroutine(PlayerStart());
            }
        }

        private void OnPlayerMoved()
        {
            _playersTurn = false;
            _enemy.MoveAStep();
            CheckGameState();
        }

        private void OnEnemyMoved()
        {
            _playersTurn = true;
            _player.BeginTurn();
            CheckGameState();
        }

        private void CheckGameState()
        {
            var tileMapper = TileMapper.Instance;
            var playerPos = _player.transform.position;
            var enemyPos = _enemy.transform.position;
            var playerCell = tileMapper.WorldPosToCell(playerPos);
            var enemyCell = tileMapper.WorldPosToCell(enemyPos);
            
            if (playerCell.Equals(enemyCell))
            {
                Debug.Log("Game over");
                _running = false;
                _player.ResetInternals(new Vector3(-2, -1.346154f, 0));
                _enemy.ResetInternals(new Vector3(2, 1.730769f, 0));
            }

            var tile = tileMapper.GetTileAt(playerCell.Value);
            if (tile.GetType() == typeof(GoalTile))
            {
                Debug.Log("Game Won");
            }
        }
    }
}