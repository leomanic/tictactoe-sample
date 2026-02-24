using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public struct SignupData
{
    public string username;
    public string nickname;
    public string password;
}

public struct LoginData
{
    public string username;
    public string password;
}

public struct LoginResult
{
    public string message;
}

public struct ScoreResult
{
    public int score;
}

public enum PostEndPoint{
    Home,
    User_SignUp,
    User_SignIn,
    User_SignOut,
    User_Score
}

public static class PostEndPointExtensions
{
    private static string[] Paths = {
        "/",
        "/users/signup",
        "/users/signin",
        "/users/signout",
        "/users/score"
    };
    
    public static string GetUrl(this PostEndPoint endpoint)
    {
        return Constants.ServerURL + Paths[(int)endpoint]; 
    }
}

public class NetworkManager : Singleton<NetworkManager>
{
    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 회원가입을 위한 함수
    /// </summary>
    /// <param name="signupData">회원가입에 필요한 정보</param>
    /// <return></return>
    public IEnumerator Signup(SignupData signupData, Action success, Action failure)
    {
        string jsonString = JsonUtility.ToJson(signupData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest www = new UnityWebRequest(PostEndPoint.User_SignUp.GetUrl(), UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError 
                || www.result == UnityWebRequest.Result.ProtocolError) 
            {
                // 오류 코드별 처리
                if (www.responseCode == 400)
                {
                    // 400 오류 발생 팝업 표시
                    GameManager.Instance.OpenConfirmPanel("필수 요소가 누락되었습니다.", () =>
                    {
                        failure?.Invoke();
                    });
                }
                else if (www.responseCode == 409)
                {
                    // 401 오류 발생 팝업 표시
                    GameManager.Instance.OpenConfirmPanel("이미 가입된 아이디입니다.", () =>
                    {
                        failure?.Invoke();
                    });
                } else {
                    GameManager.Instance.OpenConfirmPanel("회원 가입이 실패했습니다.", () =>
                    {
                        failure?.Invoke();
                    });
                }
            }
            else
            {
                var result = www.downloadHandler.text;
                Debug.Log("Result: " + result);

                // 회원 가입 성공 팝업을 표시
                GameManager.Instance.OpenConfirmPanel("회원 가입이 성공적으로 완료되었습니다.", () =>
                {
                    success?.Invoke();
                });
            }
        }
    }

    /// <summary>
    /// 로그인을 위한 함수
    /// </summary>
    /// <param name="loginData">로그인에 필요한 정보</param>
    /// <param name="success">로그인 성공 시 호출할 함수</param>
    /// <param name="failure">로그인 실패 시 호출할 함수</param>
    /// <returns></returns>
    public IEnumerator Signin(LoginData loginData, Action success, Action failure)
    {
        string jsonString = JsonUtility.ToJson(loginData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest www = new UnityWebRequest(PostEndPoint.User_SignIn.GetUrl(), UnityWebRequest.kHttpVerbPOST))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError 
                || www.result == UnityWebRequest.Result.ProtocolError) 
            {
                // 오류 코드별 처리
                if (www.responseCode == 400)
                {                    
                    GameManager.Instance.OpenConfirmPanel("로그인에 실패했습니다.", () =>
                    {
                        failure?.Invoke();
                    });
                } 
                else if (www.responseCode == 409)
                {
                    
                }
            }
            else
            {
                var cookie = www.GetResponseHeader("Set-Cookie");
                if (!string.IsNullOrEmpty(cookie))
                {
                    // 쿠키 저장
                    int lastIndex = cookie.LastIndexOf(";");
                    string sid = cookie.Substring(0, lastIndex);
                    PlayerPrefs.SetString("SID", sid);                    
                }
                var resultString = www.downloadHandler.text;
                var result = JsonUtility.FromJson<LoginResult>(resultString);
                Debug.Log("Result: " + resultString);

                // 로그인 성공 팝업을 표시
                GameManager.Instance.OpenConfirmPanel("로그인이 완료되었습니다.", () =>
                {
                    success?.Invoke();
                });
            }
        }
    }

    /// <summary>
    /// 스코어를 불러오기 위한 함수
    /// </summary>
    /// <param name="success"></param>
    /// <param name="failure"></param>
    /// <returns></returns>
    public IEnumerator GetScore(Action<ScoreResult> success, Action failure)
    {
        using (UnityWebRequest www = new UnityWebRequest(PostEndPoint.User_Score.GetUrl(), UnityWebRequest.kHttpVerbGET))
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            string sid = PlayerPrefs.GetString("SID", null);
            if(!string.IsNullOrEmpty(sid))
            {
                www.SetRequestHeader("Cookie", sid);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError
                || www.result == UnityWebRequest.Result.ProtocolError)
            {
                // 오류 코드별 처리
                if (www.responseCode == 400)
                {
                    
                }

                failure?.Invoke();
            }
            else
            {
                var resultString = www. downloadHandler.text;
                var result = JsonUtility.FromJson<ScoreResult>(resultString);
                Debug.Log("Score: " + result.score);

                success?.Invoke(result);
            }
            
        }
        yield return null;
    }
    
    public IEnumerator Logout(Action<LoginResult> success, Action failure)
    {
        using (UnityWebRequest www = new UnityWebRequest(PostEndPoint.User_SignOut.GetUrl(), UnityWebRequest.kHttpVerbGET))
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            string sid = PlayerPrefs.GetString("SID", null);
            if(!string.IsNullOrEmpty(sid))
            {
                www.SetRequestHeader("Cookie", sid);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError
                || www.result == UnityWebRequest.Result.ProtocolError)
            {
                // 오류 코드별 처리
                if (www.responseCode == 400)
                {
                    
                }

                failure?.Invoke();
            }
            else
            {
                var resultString = www. downloadHandler.text;
                var result = JsonUtility.FromJson<LoginResult>(resultString);
                Debug.Log("Score: " + result.message);

                success?.Invoke(result);
            }
            
        }
        yield return null;
    }
}
