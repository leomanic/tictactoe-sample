using UnityEngine;

public class PlayerState : BaseState
{

    private Constants.PlayerType _playerType;

    // 멀티 플레이 관련 변수
    private bool _isMultiplayer;
    private MultiplayManager _multiplayManager;
    private string _multiplayRoomId;

    public PlayerState(bool isFirstPlayer)
    {
        _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
        _isMultiplayer = false;
    }
    public PlayerState(bool isFirstPlayer, MultiplayManager multiplayManager, string roomId)
    {
        _playerType = isFirstPlayer ? Constants.PlayerType.Player1 : Constants.PlayerType.Player2;
        _isMultiplayer = true;
        _multiplayManager = multiplayManager;
        _multiplayRoomId = roomId;
    }

    // Change Turn
    public override void HandleMove(GameLogic gameLogic, int index)
    {
        ProcessMove(gameLogic, index, _playerType);
        
        // When multiplay, Transfer position info to other player
        if (_isMultiplayer)
        {
            _multiplayManager.SendPlayerMove(_multiplayRoomId, index);            
        }
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

        // Update Game O/X UI
        GameManager.Instance.SetGameTurn(_playerType);
    }

    public override void OnExit(GameLogic gameLogic)
    {
        gameLogic.blockController.onBlockClicked = null;
    }
}