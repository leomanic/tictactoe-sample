using UnityEngine;

public abstract class BaseState
{
    public abstract void OnEnter(GameLogic gameLogic);         // Call when status enter
    public abstract void HandleMove(GameLogic gameLogic, int index);    // Move player process
    public abstract void HandleNextTurn(GameLogic gameLogic);  // Call when status exit
    public abstract void OnExit(GameLogic gameLogic);          // Next turn process

    public void ProcessMove(GameLogic gameLogic, int index, Constants.PlayerType playerType)
    {
        // Mark marker at specific point
        if(gameLogic.PlaceMarker(index, playerType))    // Check true
        {
            // Check game win or loss
            var gameResult = gameLogic.CheckGameResult();
            if (gameResult == GameLogic.GameResult.None)
            {
                // Change Turn
                HandleNextTurn(gameLogic);
                Debug.Log("== Change Turn ==");
            } 
            else
            {
                // Process to Game Over
                gameLogic.GameOver(gameResult);
                Debug.Log("== Game Over ==");
            }
        }
    }
}