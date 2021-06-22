using System;
using System.Linq;
using Prototype01;
using UnityEngine;

namespace Prototype02
{
    public class EnemyController : MonoBehaviour
    {
        public float speed = 2.0f;
        public event Action enemyMoved;
        
        [SerializeField] private Transform _player;
        [SerializeField] private PathFinder _pathFinder;
        [SerializeField] private TileMapper _tileMapper;

        public void ResetInternals(Vector3 startPos)
        {
            transform.position = startPos;
        }
        
        public void MoveAStep()
        {
            var fullPath = _pathFinder.FindPath(transform.position, _player.position);
            var firstStep = fullPath.FirstOrDefault();
            if (firstStep != null)
            {
                var target = _tileMapper.CellToWorldPos(firstStep.Cell);
                if (target != null)
                {
                    this.StartMoveToPosition(target.Value, speed, enemyMoved);
                }
            }
        }
    }
}