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
        private static readonly Vector3 StartTileHighlightOffset = Vector3.up * (6.653846f - 6.538462f);
        
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
            if (!_startTileHighlight)
            {
                _startTileHighlight = _tileMapper.GetGameObject(_player.transform.position).GetComponent<TileHighlight>();
                var offset = StartTileHighlightOffset;
                _startTileHighlight.transform.position += offset;
            }
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

    public class EnemyTurnState : State
    {
        private EnemyController _enemy;
        private GameController _gameController;

        public EnemyTurnState(GameController gameController)
        {
            _enemy = gameController.Enemy;
            _gameController = gameController;
        }

        public override IEnumerator Init()
        {
            _enemy.enemyMoved += OnEnemyMoved;
            _enemy.MoveAStep();
            return base.Init();
        }

        private void OnEnemyMoved()
        {
            var tileMapper = TileMapper.Instance;
            var playerPos = _gameController.Player.transform.position;
            var enemyPos = _enemy.transform.position;
            var playerCell = tileMapper.WorldPosToCell(playerPos);
            var enemyCell = tileMapper.WorldPosToCell(enemyPos);
            
            if (playerCell.Equals(enemyCell))
            {
                _gameController.SetState(new GameOverState(_gameController));
            }
            else
            {
                _gameController.SetState(new PlayerTurnState(_gameController));
            }
            _enemy.enemyMoved -= OnEnemyMoved;
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
                _gameController.SetState(new GameOverState(_gameController));
            }
            else
            {
                var tile = tileMapper.GetTileAt(playerCell.Value);
                if (tile.GetType() == typeof(GoalTile))
                {
                    _gameController.SetState(new GameWonState(_gameController));
                }
                else
                {
                    _gameController.SetState(new EnemyTurnState(_gameController));
                }
            }
            _player.playerMoved -= OnPlayerMoved;
        }
    }
    
    public class GameOverState : State
    {
        private GameController _gameController;

        public GameOverState(GameController gameController)
        {
            _gameController = gameController;
        }

        public override IEnumerator Init()
        {
            Debug.Log("Game over");
            yield return new WaitForSeconds(2.0f);
            _gameController.SetState(new StartState(_gameController));
        }
    }

    public class GameWonState : State
    {
        private GameController _gameController;

        public GameWonState(GameController gameController)
        {
            _gameController = gameController;
        }
        
        public override IEnumerator Init()
        {
            Debug.Log("Won");
            yield return new WaitForSeconds(2.0f);
            _gameController.SetState(new StartState(_gameController));
        }
    }
}