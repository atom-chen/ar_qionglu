using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager> {


    /// <summary>
    /// 加载主场景
    /// </summary>
    public void LoadMainScene()
    {
        SceneManager.LoadScene("main");
    }
    /// <summary>
    /// 加载登陆场景
    /// </summary>
    public void LoadLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

}
