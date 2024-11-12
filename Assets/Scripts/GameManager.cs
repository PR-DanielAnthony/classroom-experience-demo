using System;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Image background;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Transform classroomTransform;

    public User User { get; set; }
    public Clipboard Clipboard { get; private set; }
    public Transform ClassroomTransform => classroomTransform;

    private readonly string getJson = "URL for connecting to a php script that returns a JSON format string"; // url/get_json.php for example
    // contact me if you wish to know more

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(GetJson(getJson));
    }

    public string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    private IEnumerator GetJson(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            Debug.LogError(www.error);

        else
            Clipboard = JsonUtility.FromJson<Clipboard>(www.downloadHandler.text);
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = background.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            background.color = color;
            yield return null;
        }

        color.a = 0f;
        background.color = color;
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = background.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            background.color = color;
            yield return null;
        }

        color.a = 1f;
        background.color = color;
    }

    public IEnumerator LoadAssetBundle(string url, Transform transform, Vector3 position = default, Quaternion rotation = default, Vector3 scale = default)
    {
        float startTime = Time.realtimeSinceStartup;

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load asset bundle: " + www.error);
                yield break;
            }

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            AssetBundleRequest request = bundle.LoadAssetAsync(bundle.GetAllAssetNames()[0], typeof(GameObject));
            yield return request;

            GameObject prefab = request.asset as GameObject;
            var newRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            GameObject instance = Instantiate(prefab, position, newRotation, transform);

            if (scale == default)
                scale = Vector3.one;

            instance.transform.localScale = scale;
            bundle.Unload(false);
            www.Dispose();
        }

        float endTime = Time.realtimeSinceStartup;
        float elapsedTime = endTime - startTime;
        Debug.Log($"Object web request took: {elapsedTime}s");
    }
}