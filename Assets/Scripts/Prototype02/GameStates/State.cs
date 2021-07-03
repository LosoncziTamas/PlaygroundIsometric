using System.Collections;
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
}