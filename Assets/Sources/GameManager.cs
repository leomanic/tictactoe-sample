using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject settingsPanelPrefab;
    private Canvas _canvas;

    // Type of Game (Single, Dual)
    private GameType _gameType;

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        _canvas = FindFirstObjectByType<Canvas>();
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
