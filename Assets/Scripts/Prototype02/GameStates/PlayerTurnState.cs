using System.Collections;
using Prototype01;

namespace Prototype02.GameStates
{
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
}