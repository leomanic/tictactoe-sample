using UnityEngine;

public class GamePanelController : MonoBehaviour
{
    // Click to Back button 
    public void OnClickBackButton()
    {
        Debug.Log("OnClick Back Button");
        GameManager.Instance.ChangeToMainScene();
    }

    // 
    // public void OnClickSettingsButton()
    // {
    //     PlayerPrefs.SetInt("game-type",0);
    //     SceneManager.LoadScene("Game")
    // }

    // Show Settings popup
    public void OnClickSettingsButton()
    {
        Debug.Log("OnClick Setting Button");
        GameManager.Instance.OpenSettingsPanel();
    }
}
