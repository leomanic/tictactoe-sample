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
             var signinPanelObject = Instantiate(signinPanelPrefab, transform);
            signinPanelObject.GetComponent<LoginPanelController>().Show(() =>
            {
                
            });
        }
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

    // 싱글 플레이
    public void OnClickSinglePlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameType.SinglePlay);
    }

    // 2인 플레이
    public void OnClickDualPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameType.DualPlay);
    }

    // 멀티 플레이
    public void OnClickMultiPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameType.MultiPlay);
    }

    // 설정 팝업
    public void OnClickSettingsButton()
    {
        GameManager.Instance.OpenSettingsPanel();
    }
}
