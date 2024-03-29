﻿using com.moblink.unity3d;
using System.IO;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Root : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        InitCreateFile();
    }

    private void Start()
    {
        Loom.Initialize();
        RequestCaptureRcode();
    }

    private void RequestCaptureRcode()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("requestCaptureRecode");

#endif
    }

    #region 全局的场景还原监听函数

    public static MobLinkScene tempScene = null;

    /// <summary>
    /// 推送的回调
    /// </summary>
    /// <param name="message"></param>
    public void Notify(string jsonData)
    {
        Debug.Log("点击了通知!!!,传回的消息是    " +jsonData);

        JsonData data = JsonMapper.ToObject(jsonData);

        float id = float.Parse(data["dbid"].ToString());
        string type = data["type"].ToString();


        Debug.Log("id===" + id + "------type==" + type);
        //TODO
        //跳转到gpsConvert
        webrequest.instance.ShowdirectPoint(type,id);


    }


    /// <summary>
    /// 全局的场景还原监听函数
    /// </summary>
    /// <param name="scene"></param>
    protected static void OnRestoreScene(MobLinkScene scene)
    {
        if (scene != null)
        {
            tempScene = scene;
            SceneManager.LoadScene(tempScene.customParams["sceneName"].ToString());
        }
    }

    #endregion 全局的场景还原监听函数

    private void OnGUI()
    {
        //检测鼠标事件
        HardWareManager.Instance.MouseLoop();
    }

    private void Update()
    {
        //检测键盘事件
        HardWareManager.Instance.KeyboardLoop();
        TimerManager.Instance.Loop();
    }







    /// <summary>
    /// 初始化创建文件/文件夹
    /// 1、Token文件
    /// </summary>
    private void InitCreateFile()
    {
        //若token.josn存在
        if (!File.Exists(PublicAttribute.TokenFilePath))
        {
            if (!Directory.Exists(Path.GetDirectoryName(PublicAttribute.TokenFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PublicAttribute.TokenFilePath));
            }
            FileStream fs = new FileStream(PublicAttribute.TokenFilePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();
        }
    }
}