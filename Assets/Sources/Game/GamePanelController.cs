using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class GamePanelController : MonoBehaviour
{
    [SerializeField] private  Image playerA_Image;
    [SerializeField] private  Image playerB_Image;

    // Click to Back button 
    public void OnClickBackButton()
    {
        Debug.Log("OnClick Back Button");
        GameManager.Instance.OpenConfirmPanel("게임을 종료합니다.", () =>
        {
            GameManager.Instance.ChangeToMainScene();            
        });
    }

    // Show Settings popup
    public void OnClickSettingsButton()
    {
        Debug.Log("OnClick Setting Button");
        GameManager.Instance.OpenSettingsPanel();
    }

    public void SetPlayerTurnPanel(PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.None:
                playerA_Image.color = Color.white;
                playerB_Image.color = Color.white;
                break;
            case PlayerType.Player1:
                playerA_Image.color = Color.blue;
                playerB_Image.color = Color.white;
                break;
            case PlayerType.Player2:
                playerA_Image.color = Color.white;
                playerB_Image.color = Color.blue;
                break;
        }
    }
}
