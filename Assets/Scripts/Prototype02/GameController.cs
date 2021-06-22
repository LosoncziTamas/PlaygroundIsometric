using Prototype01;
using UnityEngine;

namespace Prototype02
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private EnemyController _enemy;

        private bool _running;
        private bool _playersTurn;
        
        //TODO: add obstacle

        private void Start()
        {
            _player.playerMoved += OnPlayerMoved;
            _enemy.enemyMoved += OnEnemyMoved;
        }

        private void OnGUI()
        {
            if (!_running && GUILayout.Button("Start Game"))
            {
                _running = true;
                _playersTurn = true;
                _player.BeginTurn();
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
            var playerPos = _player.transform.position;
            var enemyPos = _enemy.transform.position;
            var playerCell = TileMapper.Instance.WorldPosToCell(playerPos);
            var enemyCell = TileMapper.Instance.WorldPosToCell(enemyPos);
            
            if (playerCell.Equals(enemyCell))
            {
                Debug.Log("Game over");
                _running = false;
                _player.ResetInternals(new Vector3(-2, -1.346154f, 0));
                _enemy.ResetInternals(new Vector3(2, 1.730769f, 0));
            }
        }
    }
}