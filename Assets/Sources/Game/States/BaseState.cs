using UnityEngine;

public abstract class BaseState
{
    public abstract void OnEnter(GameLogic gameLogic);         // Call when status enter
    public abstract void HandleMove(GameLogic gameLogic, int index);      // Move player process
    public abstract void HandleNextTurn(GameLogic gameLogic);  // Call when status exit
    public abstract void OnExit(GameLogic gameLogic);          // Next turn process

    public void ProcessMove(GameLogic gameLogic, int index, Constants.PlayerType playerType)
    {
        // Mark marker at specific point
        if(gameLogic.PlaceMarker(index, playerType))    // check true
        {
            // Turn switch
            HandleNextTurn(gameLogic);
        }
    }
}