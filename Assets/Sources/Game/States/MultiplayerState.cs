using UnityEngine;

public class MultiplayerState : BaseState
{

    private Constants.PlayerType _playerType;
    private MultiplayManager _multiplayManager;

    public MultiplayerState(bool isFirstPlayer, MultiplayManager multiPlayManager)
    {
        _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
        _multiplayManager = multiPlayManager;
    }
    
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
        _multiplayManager.OnOpponentMove = (moveData) =>
        {
            if (moveData.position >=0 && moveData.position < Constants.BOARD_SIZE * Constants.BOARD_SIZE)
            {
                HandleMove(gameLogic,moveData.position);
                // Update Game O/X UI
                GameManager.Instance.SetGameTurn(_playerType);
            }
            else
            {
                // TODO : 유효하지 않은 이동 데이터 처리
            }
        };
    }

    public override void OnExit(GameLogic gameLogic)
    {
        _multiplayManager.OnOpponentMove = null;
    }
}