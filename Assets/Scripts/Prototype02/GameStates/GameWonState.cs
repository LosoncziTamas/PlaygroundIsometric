using System.Collections;
using UnityEngine;

namespace Prototype02.GameStates
{
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