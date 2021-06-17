using System;
using System.Collections;
using System.Linq;
using Prototype01;
using UnityEngine;

namespace Prototype02
{
    public class EnemyController : MonoBehaviour
    {
        private const float DistanceThreshold = 0.01f;

        public float speed = 2.0f;
        
        [SerializeField] private Transform _player;
        [SerializeField] private PathFinder _pathFinder;
        [SerializeField] private TileMapper _tileMapper;

        private void OnGUI()
        {
            GUILayout.Space(100);
            if (GUILayout.Button("Move enemy"))
            {
                MoveAStep();
            }
        }

        public void MoveAStep()
        {
            var fullPath = _pathFinder.FindPath(transform.position, _player.position);
            var firstStep = fullPath.FirstOrDefault();
            if (firstStep != null)
            {
                StartCoroutine(MoveToCell(firstStep.Cell));
            }
        }

        private IEnumerator MoveToCell(Vector3Int cell)
        {
            var target = _tileMapper.CellToWorldPos(cell);
            if (target == null)
            {
                yield break;
            }

            var distance = Vector3.Distance(target.Value, transform.position);
            while (distance > DistanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.Value, Time.fixedDeltaTime * speed);
                yield return new WaitForFixedUpdate();
                distance = Vector3.Distance(target.Value, transform.position);
            }

        }
    }
}