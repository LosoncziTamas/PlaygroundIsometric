using System.Collections;
using Prototype01;
using UnityEngine;

namespace Prototype02
{
    public abstract class State
    {
        public virtual IEnumerator Init()
        {
            yield break;
        }

        public virtual IEnumerator PlayerTurn()
        {
            yield break;
        }
        
        public virtual IEnumerator EnemyTurn()
        {
            yield break;
        }
        
        public virtual IEnumerator Won()
        {
            yield break;
        }
        
        public virtual IEnumerator Lost()
        {
            yield break;
        }
    }

    public abstract class StateMachine : MonoBehaviour
    {
        protected State State;

        public void SetState(State newState)
        {
            State = newState;
            StartCoroutine(newState.Init());
        }
    }

    public class StartState : State
    {
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
        
        public override IEnumerator Init()
        {
            yield return null;
            _startTileHighlight = _tileMapper.GetGameObject(_player.transform.position).GetComponent<TileHighlight>();
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
            _gameController.SetState(new PlayerTurnState(_gameController));
        }
    }

    public class EnemyTurnState : State
    {
        private EnemyController _enemy;
        private GameController _gameController;

        public EnemyTurnState(GameController gameController)
        {
            _enemy = gameController.Enemy;
            _gameController = gameController;
        }

        public override IEnumerator EnemyTurn()
        {
            _enemy.MoveAStep();
            var tileMapper = TileMapper.Instance;
            var playerPos = _gameController.Player.transform.position;
            var enemyPos = _enemy.transform.position;
            var playerCell = tileMapper.WorldPosToCell(playerPos);
            var enemyCell = tileMapper.WorldPosToCell(enemyPos);
            
            if (playerCell.Equals(enemyCell))
            {
                //TODO: game over
            }
            else
            {
                _gameController.SetState(new PlayerTurnState(_gameController));
            }
            
            yield break;
        }
    }

    public class PlayerTurnState : State
    {
        private GameController _gameController;
        private EnemyController _enemy;
        private PlayerController _player;
        
        public PlayerTurnState(GameController gameController)
        {
            _gameController = gameController;
            _enemy = gameController.Enemy;
            _player = gameController.Player;
        }

        public override IEnumerator Init()
        {
            _player.playerMoved += OnPlayerMoved;
            _player.BeginTurn();
            return base.Init();
        }

        private void OnPlayerMoved()
        {
            var tileMapper = TileMapper.Instance;
            var playerPos = _player.transform.position;
            var enemyPos = _enemy.transform.position;
            var playerCell = tileMapper.WorldPosToCell(playerPos);
            var enemyCell = tileMapper.WorldPosToCell(enemyPos);
            
            if (playerCell.Equals(enemyCell))
            {
                //TODO: game over
            }
            else
            {
                var tile = tileMapper.GetTileAt(playerCell.Value);
                if (tile.GetType() == typeof(GoalTile))
                {
                    //TODO: won
                }
                else
                {
                    _gameController.SetState(new EnemyTurnState(_gameController));
                }
            }
            _player.playerMoved -= OnPlayerMoved;
        }
    }
}