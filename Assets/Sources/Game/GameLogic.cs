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
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter(this);
    }

    public void PlaceMarker(int index, PlayerType playerType)
    {
        blockController.PlaceMarker(index, playerType);
    }
}