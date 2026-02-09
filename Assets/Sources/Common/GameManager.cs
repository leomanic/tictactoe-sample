using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject settingsPanelPrefab;
    private Canvas _canvas;

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

            // Create game logic
            _gameLogic = new GameLogic(GameType.DualPlay, blockController);
        }
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

    // Scene transition
    public void ChangeToGameScene(GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene(SCENE_GAME);        
    }

    public void ChangeToMainScene()
    {
        SceneManager.LoadScene(SCENE_MAIN);        
    }
}
