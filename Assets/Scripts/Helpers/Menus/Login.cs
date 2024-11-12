using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : Menu
{
    [SerializeField] private InputField usernameInputField;
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button skipLoginButton;
    [SerializeField] private Text messageText;

    private readonly string loginURL = "Insert URL for connecting to a php script"; // url/login.php for example
    // contact me if you wish to know more

    public override void Initialize()
    {
        loginButton.onClick.RemoveAllListeners();
        skipLoginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(() => StartCoroutine(LoginUser(usernameInputField.text, passwordInputField.text)));
        skipLoginButton.onClick.AddListener(() => StartCoroutine(CloseMenu()));
    }

    private IEnumerator LoginUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password))
        {
            messageText.text = "You cannot login with an empty text box!";
            yield break;
        }

        WWWForm loginForm = new();
        loginForm.AddField("loginUser", username);
        loginForm.AddField("loginPass", password);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(loginURL, loginForm))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = loginURL.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.InProgress:
                    messageText.text = "Logged in...now loading...";
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    User user = null;

                    try
                    {
                        user = GameManager.Instance.User = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);
                    }
                    catch
                    {
                        messageText.text = "Invalid username or password";
                        throw;
                    }

                    messageText.text = "Logged in!\n";
                    yield return CloseMenu();
                    break;
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    messageText.text = "Could not connect.";
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    messageText.text = "Could not connect.";
                    break;
            }
        }
    }

    private IEnumerator CloseMenu()
    {
        yield return GameManager.Instance.FadeOut();
        menuManager.Show<AssetBundleLoader>();
    }
}