using static Constants;
using UnityEngine;

public class GameLogic
{
    public BlockController blockController;

    // Status board
    private PlayerType[,] _board;
    public BaseState playerAState;
    public BaseState playerBState;

    // A variable that represents the current state
    private BaseState _currentState;

    private MultiplayManager _multiplayManager;
    private string _multiplayRoomId;

    // Result of Game
    public enum GameResult {None, Win, Lose, Draw}
    // Board Info    
    public Constants.PlayerType[,] Board { get { return _board; }}
    public GameLogic(GameType gameType, BlockController blockController)
    {
        // Allocate BlockController 
        this.blockController = blockController;

        // initalize board info
        _board = new PlayerType[BOARD_SIZE, BOARD_SIZE];

        switch (gameType)
        {
            case GameType.SinglePlay:
                // Single-player mode initialization task
                playerAState = new PlayerState(true);
                playerBState = new AIState(false);

                // Initial State Setup (Start Player A)
                SetState(playerAState);

                break;
            case GameType.DualPlay:
                // Dual-player mode initialization task
                playerAState = new PlayerState(true);
                playerBState = new PlayerState(false);
                
                // Initial State Setup (Start Player A)
                SetState(playerAState);
                break;
            case GameType.MultiPlay:
                // Multi-player mode initialization task
                _multiplayManager = new MultiplayManager((state, roomId) =>
                {
                    _multiplayRoomId = roomId;
                    switch (state)
                    {
                        case MultiplayManagerState.CreateRoom:
                            Debug.Log("방 생성됨, 방 ID " + roomId);
                            break;

                        case MultiplayManagerState.JoinRoom:
                            Debug.Log("방 참가됨, 방 ID " + roomId);
                            playerAState = new MultiplayerState(true, _multiplayManager);
                            playerBState = new PlayerState(false);
                            SetState(playerAState);
                            break;

                        case MultiplayManagerState.StartGame:
                            Debug.Log("게임 시작됨, 방 ID " + roomId);
                            playerAState = new PlayerState(true, _multiplayManager, roomId);
                            playerBState = new MultiplayerState(false, _multiplayManager);
                            SetState(playerAState);
                            break;

                        case MultiplayManagerState.ExitRoom:
                            Debug.Log("본인이 방을 나감, 방 ID " + roomId);
                            break;

                        case MultiplayManagerState.EndGame:
                            Debug.Log("상대방이 접속 끊음, 방 ID " + roomId);
                            break;
                    }
                });
                break;
        }
    }

    // Call by method when turn changed (State transition method)
    public void SetState(BaseState newState)
    {
        _currentState?.OnExit(this);
        _currentState = newState;
        _currentState.OnEnter(this);
    }

    // Method for Mark marker
    public bool PlaceMarker(int index, PlayerType playerType)
    {
        // Convert coordinate values to index values
        var row = index / BOARD_SIZE;
        var col = index % BOARD_SIZE;

        // Prevent duplication clicks
        if (_board[row, col] != Constants.PlayerType.None) return false;

        blockController.PlaceMarker(index, playerType);

        _board[row, col] = playerType;

        return true;
    }

    // Change Turn
    public void ChangeGameState()
    {
        if (_currentState == playerAState)
        {
            SetState(playerBState);
        }
        else
        {
            SetState(playerAState);
        }
    }

    // Check game results
    public GameResult CheckGameResult()
    {
        // Implementing logic to check victory conditions

        if (TicTacToeAI.CheckGameWin(PlayerType.Player1, _board)) {return GameResult.Win;}
        if (TicTacToeAI.CheckGameWin(PlayerType.Player2, _board)) {return GameResult.Lose;}
        if (TicTacToeAI.CheckGameDraw(_board)) {return GameResult.Draw;}
        return GameResult.None;
    }

 

    // Process to Game Over
    public void GameOver(GameResult gameResult)
    {
        string resultStr = "";
        switch (gameResult)
        {
            case GameResult.Win:
                resultStr = "Player1 승리";
                break;
            case GameResult.Lose:
                resultStr = "Player2 승리";
                break;
            case GameResult.Draw:
                resultStr = "무승부";
                break;
        }
        // TODO: 게임 오버 팝업을 띄우고, 팝업에서 확인 버튼 누르면 Main 씬으로 전환
        GameManager.Instance.OpenConfirmPanel(resultStr, () =>
        {
           GameManager.Instance.ChangeToMainScene(); 
        });
    }
}