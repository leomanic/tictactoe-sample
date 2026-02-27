using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private GameObject confirmPanelPrefab;
    private Canvas _canvas;

    // Game UI Controller    
    private GamePanelController _gamePanelController;

    // Game Logic
    private GameLogic _gameLogic;

    // Type of Game (Single, Dual)
    private GameType _gameType;

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        // refer to Cavas from new Scene
        _canvas = FindFirstObjectByType<Canvas>();

        Debug.Log("::: OnSceneLoad()");

        if (scene.name == SCENE_GAME)
        {
            var blockController = FindFirstObjectByType<BlockController>();
            if (blockController != null)
            {
                blockController.InitBlocks();
            }

            // refer to GamePanelController
            _gamePanelController = FindFirstObjectByType<GamePanelController>();

            // Create game logic
            _gameLogic = new GameLogic(_gameType, blockController);
        }
    }

    // Update Game O/X UI
    public void SetGameTurn(PlayerType playerType)
    {
        _gamePanelController.SetPlayerTurnPanel(playerType);
    }

    // Open to Settings Panel
    public void OpenSettingsPanel()
    {
        if (_canvas == null)
        {
            return;
        }
        var settingsPanelObject = Instantiate(settingsPanelPrefab, _canvas.transform);
        settingsPanelObject.GetComponent<SettingsPanelController>().Show();
    }

    // Open to Confirm Panel
    public void OpenConfirmPanel(string message, ConfirmPanelController.OnConfirmButtonClicked onConfirmButtonClicked)
    {
        var confirmPanelObject = Instantiate(confirmPanelPrefab, _canvas.transform);
        confirmPanelObject.GetComponent<ConfirmPanelController>().Show(message, onConfirmButtonClicked);
      }

    // Scene transition
    public void ChangeToGameScene(GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene(SCENE_GAME);        
    }

    public void ChangeToMainScene()
    {
        _gameLogic?.Dispose();
        _gameLogic = null;
        SceneManager.LoadScene(SCENE_MAIN);        
    }
}
