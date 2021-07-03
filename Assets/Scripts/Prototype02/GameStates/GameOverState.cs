using System.Collections;
using UnityEngine;

namespace Prototype02.GameStates
{
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
}