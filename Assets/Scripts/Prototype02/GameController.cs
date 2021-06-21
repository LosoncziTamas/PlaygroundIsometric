using System;
using UnityEngine;

namespace Prototype02
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private EnemyController _enemy;

        private bool _running;
        private bool _playersTurn;

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
        }

        private void OnEnemyMoved()
        {
            _playersTurn = true;
            _player.BeginTurn();
        }
    }
}