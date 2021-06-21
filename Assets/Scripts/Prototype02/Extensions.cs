using System;
using System.Collections;
using UnityEngine;

namespace Prototype02
{
    public static class Extensions
    {
        private const float DistanceThreshold = 0.01f;
        
        public static IEnumerator MoveToPosition(this MonoBehaviour behaviour, Vector3 target, float speed, Action onComplete = null)
        {
            var distance = Vector3.Distance(target, behaviour.transform.position);
            while (distance > DistanceThreshold)
            {
                behaviour.transform.position = Vector3.MoveTowards(behaviour.transform.position, target, Time.fixedDeltaTime * speed);
                yield return new WaitForFixedUpdate();
                distance = Vector3.Distance(target, behaviour.transform.position);
            }
            onComplete?.Invoke();
        }

        public static void StartMoveToPosition(this MonoBehaviour behaviour, Vector3 target, float speed, Action onComplete = null)
        {
            behaviour.StartCoroutine(behaviour.MoveToPosition(target, speed, onComplete));
        }
    }
}