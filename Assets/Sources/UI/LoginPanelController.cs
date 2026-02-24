using UnityEngine;
using TMPro;

public class LoginPanelController : PanelController
{
    [SerializeField] private TMPro.TMP_InputField usernameInputField;
    [SerializeField] private TMPro.TMP_InputField passwordInputField;

    // 로그인 팝업에서 "확인" 버튼 클릭시 동작할 함수를 전달받기 위한 델리게이트
    public delegate void OnLoginButtonClicked();
    private OnLoginButtonClicked _onLoginButtonClicked;

    public void Show(OnLoginButtonClicked onLoginButtonClicked)
    {
        _onLoginButtonClicked = onLoginButtonClicked;
        Show();
    }

    public void OnClickConfirmButton()
    {
        // Input Field에 입력된 값을 체크해서 서버에 전달

        var username = usernameInputField.text;
        var password = passwordInputField.text;        

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            // "입력 값이 누락되었습니다." 팝업 창 표시
            GameManager.Instance.OpenConfirmPanel("입력 값이 누락되었습니다.", () =>
            {
            });
        }

        // 로그인 진행
        var loginData = new LoginData();
        loginData.username = username;            
        loginData.password = password;

        // 서버로 SignupData를 전달하면서 
        StartCoroutine(NetworkManager.Instance.Signin(loginData, () =>
        {
            Hide();
        }, () =>
        {
            // usernameInputField.text = "";
            passwordInputField.text = "";
        }));
    }

    public void OnClickCancelButton()
    {
        Hide();
    }
}