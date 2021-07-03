using System.Collections;
using Prototype01;

namespace Prototype02.GameStates
{
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
}