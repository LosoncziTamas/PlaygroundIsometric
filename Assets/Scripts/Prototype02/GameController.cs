using Prototype01;
using UnityEngine;

namespace Prototype02
{
    public class GameController : StateMachine
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private EnemyController _enemy;
        
        public PlayerController Player => _player;
        public EnemyController Enemy => _enemy;
        public TileMapper TileMapper => TileMapper.Instance;
        
        private void Start()
        {
            _enemy.enemyMoved += OnEnemyMoved;
            SetState(new StartState(this));
        }


        private void OnEnemyMoved()
        {
            StartCoroutine(State.PlayerTurn());
        }
    }
}