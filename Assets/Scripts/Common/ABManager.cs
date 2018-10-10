using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ABManager : Singleton<ABManager> {
    public static bool uninstall;
    WWW assetbundle;
    Scene scene;
	// Use this for initialization
	void Start () {
        scene = SceneManager.GetActiveScene();
        if(scene.name== "main" && uninstall)
        {
            uninstall = false;
            UninstallAssetbundle();
        }
	}

    private IEnumerator LoadAssets(string path)
    {
        Debug.Log(path);
        assetbundle = new WWW(path);
        yield return assetbundle;
        for (int i = 0; i < assetbundle.assetBundle.GetAllAssetNames().Length; i++)
            Debug.Log(assetbundle.assetBundle.GetAllAssetNames()[i]);
        int index1 = path.LastIndexOf("/");
        int index2 = path.LastIndexOf(".");
        string suffix = path.Substring(index1 + 1, index2 - index1 - 1);
        SceneManager.LoadScene(suffix);
        assetbundle.assetBundle.Unload(false);
    }
    private void UninstallAssetbundle()
    {
        assetbundle.assetBundle.Unload(false);
        assetbundle.Dispose();
    }

    public void BackMainScene()
    {
        SceneManager.LoadScene("Main");
    }
    public void LogOut()
    {
        HttpManager.Instance.Logout((b =>
        {
            if (!b)
            {

            }
        }));
        File.Delete(PublicAttribute.LocalFilePath + "APP/Token.json");
        SceneManager.LoadScene("Login");
    }
}
