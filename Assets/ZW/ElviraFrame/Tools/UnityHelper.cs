using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.IO;
/// <summary>
/// 集成大量的通用算法
/// </summary>

public static   class UnityHelper 
    {


    static float deltaTime;


    public static UniWebView CreateWebView(string webViewUIName)
    {
        var webViewGameObject = GameObject.Find(webViewUIName);
        if (webViewGameObject == null)
        {
            webViewGameObject = new GameObject(webViewUIName);
            var webView = webViewGameObject.AddComponent<UniWebView>();
            return webView;
        }
        else
        {
            var webView = webViewGameObject.GetComponent<UniWebView>();
            return webView;
        }
    }

    /// <summary>
    /// 间隔指定时间，返回bool值，true：时间到了，false，时间没到
    /// </summary>
    /// <param name="delta"></param>
    /// <returns> </returns>
    public   static bool    GetSmallTime(float  delta)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime>=delta)
            {
                deltaTime = 0;
                return true;
            }
            else
            {
                return false; 
            }
        }
        /// <summary>
        /// 面向指定目标，sourceTransform：自身，targeTransform：目标rotationSpeed：旋转速度
        /// </summary>
        /// <param name="targeTransform"></param>
        /// <param name="sourceTransform"></param>
        /// <param name="rotationSpeed"></param>
        public static  void FaceToGoal(Transform  sourceTransform,Transform   targeTransform,float rotationSpeed)
        {

            sourceTransform.rotation =
                Quaternion.Slerp(
                    sourceTransform.rotation,
                    Quaternion.LookRotation(new Vector3(targeTransform.position.x, 0, targeTransform.position.z) - new Vector3(sourceTransform.position.x, 0, sourceTransform.position.z)),
                    rotationSpeed);
        }

       /// <summary>
       /// 返回minNum和maxNum之间的随机值，minNum：最小值，maxNum：最大值
       /// </summary>
       /// <param name="minNum"></param>
       /// <param name="maxNum"></param>
       /// <returns></returns>
        public  static int GetRandomNum(int minNum,int maxNum)
        {
            int random = 0;
            if (minNum==maxNum)
            {
                random = minNum;
            }
            random= Random.Range(minNum, maxNum+1);
            return random;
        }
    /// <summary>
    /// Get the local Assets data path for a given file path related to the data folder of your host app.
    /// This method will help you to concat a URL string for a file under you stored in the `Assets/`.
    /// </summary>
    /// <param name="path">
    /// The relative path to the Assets/    of your file.
    /// </param>
    /// <returns>The path you could use as the url for the web view.</returns>
    public static string AssetsDataURLForPath(string path)
    {
        return Path.Combine("file://" + Application.dataPath, path);
    }
    /// <summary>
    /// Get the local persistent data path for a given file path related to the data folder of your host app.
    /// This method will help you to concat a URL string for a file under you stored in the `persistentDataPath`.
    /// </summary>
    /// <param name="path">
    /// <returns>The path you could use as the url for the web view.</returns>
    public static string PersistentDataURLForPath(string path)
    {
        return Path.Combine("file://" + Application.persistentDataPath, path);
    }
    public static string StreamingAssetURLForPath(string path)
    {
#if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS) && !UNITY_EDITOR_WIN
        return Path.Combine("file://" + Application.streamingAssetsPath, path);
#elif UNITY_ANDROID
        return Path.Combine("file:///android_asset/", path);
#else
        UniWebViewLogger.Instance.Critical("The current build target is not supported.");
        return string.Empty;
#endif
    }
    /// <summary>
    /// 查找或者添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrCreateComponent<T>(this GameObject obj) where T : MonoBehaviour
    {
        T t = obj.GetComponent<T>();
        if (t == null)
        {
            t = obj.AddComponent<T>();
        }
        return t;

    }
    /// <summary>
    /// 字符转数字
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        int temp = 0;
        int.TryParse(str, out temp);
        return temp;
    }

    private static string localFilePath;

    /// <summary>
    /// app文件存放路径
    /// </summary>
    public static string LocalFilePath
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                localFilePath = Application.persistentDataPath + "/DownloadFile/";
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                localFilePath = Application.dataPath + "/DownloadFile/";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                localFilePath = Application.persistentDataPath + "/DownloadFile/";
            }
            if (!Directory.Exists(localFilePath))
            {
                Directory.CreateDirectory(localFilePath);
            }
            return localFilePath;
        }
        set { localFilePath = value; }
    }
}

