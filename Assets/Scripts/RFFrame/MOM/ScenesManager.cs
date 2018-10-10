using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : Singleton<ScenesManager> {


    /// <summary>
    /// ����������
    /// </summary>
    public void LoadMainScene()
    {
        SceneManager.LoadScene("main");
    }
    /// <summary>
    /// ���ص�½����
    /// </summary>
    public void LoadLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

}
