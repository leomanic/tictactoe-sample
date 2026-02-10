using static Constants;

public class GameLogic
{
    public BlockController blockController;

    // Status board
    private PlayerType[,] _board;
    public BaseState playerAState;
    public BaseState playerBState;

    // A variable that represents the current state
    private BaseState _currentState;

    // Result of Game
    public enum GameResult {Win, Lose, Draw, None}
    
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
                playerBState = new AIState();

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

        if (CheckGameWin(PlayerType.Player1, _board)) {return GameResult.Win;}
        if (CheckGameWin(PlayerType.Player2, _board)) {return GameResult.Lose;}
        if (CheckGameDraw(_board)) {return GameResult.Draw;}
        return GameResult.None;
    }

    // Method for checking game winning conditions
    public bool CheckGameWin(Constants.PlayerType playerType, Constants.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == playerType &&
                board[row, 1] == playerType &&
                board[row, 2] == playerType)
            {
                return true;
            }
        }

        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == playerType &&
                board[1, col] == playerType &&
                board[2, col] == playerType)
            {
                return true;
            }
        }

        if (board[0,0] == playerType &&
            board[1,1] == playerType &&
            board[2,2] == playerType)
        {
            return true;
        }

        if (board[0,2] == playerType &&
            board[1,1] == playerType &&
            board[2,0] == playerType)
        {
            return true;
        }

        return false;
    }

    public bool CheckGameDraw(Constants.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == Constants.PlayerType.None) return false;
            }
        }

        return true;
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