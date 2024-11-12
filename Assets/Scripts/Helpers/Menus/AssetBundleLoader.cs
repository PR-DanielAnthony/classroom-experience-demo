using System.Collections;
using UnityEngine;

/// <summary>
/// Transition menu for loading objects
/// </summary>
public class AssetBundleLoader : Menu
{
    private readonly string url = "Insert URL for loading Asset Bundles here"; // url/asset_bundles/classroom for example
    // contact me if you wish to know more

    public override void Show()
    {
        base.Show();
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        float startTime = Time.realtimeSinceStartup;
        yield return GameManager.Instance.LoadAssetBundle(url, GameManager.Instance.ClassroomTransform);
        float endTime = Time.realtimeSinceStartup;
        float elapsedTime = endTime - startTime;
        yield return GameManager.Instance.FadeIn();
        Debug.Log($"Room web request took: {elapsedTime}s");
        menuManager.Show<ClassroomManager>();
    }
}