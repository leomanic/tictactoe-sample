using UnityEngine;

public class PlayerState : BaseState
{

    private Constants.PlayerType _playerType;

    public PlayerState(bool isFirstPlayer)
    {
        _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
    }

    // Change Turn
    public override void HandleMove(GameLogic gameLogic, int index)
    {
        ProcessMove(gameLogic, index, _playerType);
    }

    public override void HandleNextTurn(GameLogic gameLogic)
    {
        gameLogic.ChangeGameState();
    }

    public override void OnEnter(GameLogic gameLogic)
    {
        gameLogic.blockController.onBlockClicked = (blockIndex) => 
        {
            // Process logic when Clicked block
            HandleMove(gameLogic, blockIndex);
        };
    }

    public override void OnExit(GameLogic gameLogic)
    {

    }
}