using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject settingsPanelPrefab;
    [SerializeField] private Canvas canvas;

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        
    }

    // Open to Settings Panel
    public void OpenSettingsPanel()
    {
        var settingsPanelObject = Instantiate(settingsPanelPrefab, canvas.transform);
        settingsPanelObject.GetComponent<SettingsPanelController>().Show();
    }
}
