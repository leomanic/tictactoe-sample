using UnityEngine;
using static Constants;

public class MainPanelController : MonoBehaviour
{
    [SerializeField] private GameObject signupPanelPrefab;
    [SerializeField] private GameObject signinPanelPrefab;

    // Signup panel test
    void Start()
    {
        string sid = PlayerPrefs.GetString("SID", null);

        if(string.IsNullOrEmpty(sid))
        {
            // var signupPanelObject = Instantiate(signupPanelPrefab, transform);
            // signupPanelObject.GetComponent<SignupPanelController>().Show(() =>
            // {
                
            // });
             var signinPanelObject = Instantiate(signinPanelPrefab, transform);
            signinPanelObject.GetComponent<LoginPanelController>().Show(() =>
            {
                
            });
        }
        // else {
        //     var signinPanelObject = Instantiate(signinPanelPrefab, transform);
        //     signinPanelObject.GetComponent<LoginPanelController>().Show(() =>
        //     {
                
        //     });
        // }
    }

    public void OnClickGetScore()
    {
        StartCoroutine(NetworkManager.Instance.GetScore((score) =>
        {
            GameManager.Instance.OpenConfirmPanel($"현재 점수: {score}", () =>
            {
            });
        }, () =>
        {
        }));
    }

    public void OnClickLogout()
    {
        StartCoroutine(NetworkManager.Instance.Logout((result) =>
        {
            GameManager.Instance.OpenConfirmPanel($"로그아웃: {result.message}", () =>
            {
                PlayerPrefs.DeleteKey("SID");   // 키값 삭제
            });
        }, () =>
        {

        }));
    } 

    public void OnClickSinglePlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameType.SinglePlay);
    }

    public void OnClickDualPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameType.DualPlay);
    }

    public void OnClickSettingsButton()
    {
        GameManager.Instance.OpenSettingsPanel();
    }
}
