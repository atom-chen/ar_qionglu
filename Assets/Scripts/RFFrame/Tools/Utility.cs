/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     Utility.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-25 
 *Description:    
 *History: 
*/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class Utility
{
    static void Activate(Transform t, bool compatibilityMode)
    {
        SetActiveSelf(t.gameObject, true);

        if (compatibilityMode)
        {
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                if (child.gameObject.activeSelf) return;
            }
            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                Activate(child, true);
            }
        }
    }
    static void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }
    static void Deactivate(Transform t) { SetActiveSelf(t.gameObject, false); }

    #region 公用方法
    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
    /// <summary>
    /// 查找子节点
    /// </summary>
    public static Transform FindDeepChild(GameObject _target, string _childName)
    {
        Transform resultTrs = null;
        resultTrs = _target.transform.Find(_childName);
        if (resultTrs == null)
        {
            foreach (Transform trs in _target.transform)
            {
                resultTrs = Utility.FindDeepChild(trs.gameObject, _childName);
                if (resultTrs != null)
                    return resultTrs;
            }
        }
        return resultTrs;
    }

    /// <summary>
    /// 查找子节点脚本
    /// </summary>
    public static T FindDeepChild<T>(GameObject _target, string _childName) where T : Component
    {
        Transform resultTrs = Utility.FindDeepChild(_target, _childName);
        if (resultTrs != null)
            return resultTrs.gameObject.GetComponent<T>();
        return (T)((object)null);
    }

    public static void SetGoState(GameObject go, bool state) { SetGoState(go, state, true); }

    public static void SetGoState(GameObject go, bool state, bool compatibilityMode)
    {
        if (go)
        {
            if (state)
            {
                Activate(go.transform, compatibilityMode);
            }
            else Deactivate(go.transform);
        }
    }
    /// <summary>
    /// 添加UI子节点
    /// </summary>
    public static void AddChildToTarget_UI(Transform target, Transform child, int siblingIndex, bool SetActive = true)
    {
        child.gameObject.SetActive(SetActive);
        //child.parent = target;
        child.SetParent(target, false);
        child.localScale = Vector3.one;

        child.localPosition = new Vector3(child.localPosition.x,child.localPosition.y,0);
        //child.localPosition = child.position;
        child.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //child.localPosition = child.position;
        child.localEulerAngles = Vector3.zero;
        child.SetSiblingIndex(siblingIndex);
        ChangeChildLayer(child, target.gameObject.layer);
    }
    public static void AddChildToTarget(Transform target, Transform child, bool SetActive = true)
    {
        child.gameObject.SetActive(SetActive);
        child.SetParent(target, false);
        child.localScale = Vector3.one;
        child.localPosition = child.position;
        child.localEulerAngles = Vector3.zero;
        ChangeChildLayer(child, target.gameObject.layer);
    }

    /// <summary>
    /// 修改子节点Layer
    /// </summary>
    public static void ChangeChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }

    /// <summary>
    /// 创建物体并设置物体的状态
    /// </summary>
    /// <param name="go"></param>
    /// <param name="goState"></param>
    /// <returns></returns>
    public static GameObject InstantiateGo(GameObject go,bool goState=true)
    {
        if (go == null)
        {
            return null;
        }
        GameObject Go = GameObject.Instantiate(go);
        SetGoState(Go, goState);
        return Go;
    }
    /// <summary>
    /// 销毁物体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="Delay">延时</param>
    /// <param name="action"></param>
    public static void DestoryGo(GameObject go, float Delay = 0.1f,Action action = null)
    {
        TimerManager.Instance.Register(go, Delay, () =>
        {
            TimerManager.Instance.UnRegister(go);
            UnityEngine.GameObject.Destroy(go);
        });
    }

    public static string GetMd5Hash(byte[] bytes)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        byte[] data = md5Hasher.ComputeHash(bytes);
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
    }

    public static string GetMd5Hash(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        // 设置当前流的位置为流的开始
        stream.Seek(0, SeekOrigin.Begin);
        return GetMd5Hash(bytes);
    }

    public static List<string> GetFolder(string path)
    {
        var dirs = Directory.GetDirectories(path);
        List<string> list = new List<string>();
        foreach (string item in dirs)
        {
           list.Add(Path.GetFileNameWithoutExtension(item));
        }
        return list;
    }

    /// <summary>
    /// 获取文件夹内所有的文件  仅限两个层级
    /// </summary>
    /// <param name="dir"></param>
    public static List<string> GetFiles(string dir)
    {
        var dirs = Directory.GetDirectories(dir);
        return (from itemm in dirs select new DirectoryInfo(itemm) into d from fsinfo in d.GetFileSystemInfos() select fsinfo.FullName).ToList();
    }


    /// <summary>
    /// 根据文件获取oss的下载地址
    /// vsz-video.oss-cn-beijing.aliyuncs.com/test_video.mp4?OSSAccessKeyId=LTAIX9PZ52E9mVhE&Expires=1538049678&Signature=adYWEp5zrVvp27UWOuzXhyVGxIU%3d
    /// </summary>
    /// <param name="endpoint">资源类型</param>
    /// <param name="ObjectName">资源名称</param>
    /// <returns></returns>
    public static string OSSURI(string endpoint, string ObjectName, string expire=null)
    {
        if (string.IsNullOrEmpty(expire))
        {
            expire = DateTime.UtcNow.GetDateTimeFormats('r')[0].ToString();
        }
        string uri = HmacsHa1("GET\n\n\n" + expire + "\n/" + endpoint + "/" + ObjectName,PublicAttribute.OSSAccessKeySecret);
        return uri;
        uri = endpoint + ".oss-cn-beijing.aliyuncs.com/" + ObjectName + "?OSSAccessKeyId=" +PublicAttribute.OSSAccessKeyId + "&Expires=" + expire + "&Signature=" + uri;
        Debug.Log(uri);
        return uri;
    }

    /// <summary>
    /// 获取OSS签名
    /// </summary>
    /// <param name="endpoint">资源类型</param>
    /// <param name="ObjectName">资源名称</param>
    /// <param name="expire">超期时间</param>
    /// <returns></returns>
    public static string OSSSignature(string endpoint, string ObjectName, string expire = null)
    {
        if (string.IsNullOrEmpty(expire))
        {
            expire = DateTime.UtcNow.GetDateTimeFormats('r')[0].ToString();
        }
        string Signature = HmacsHa1("GET\n\n\n" + expire + "\n/" + endpoint + "/" + ObjectName, PublicAttribute.OSSAccessKeySecret);
        Signature = "OSS " + PublicAttribute.OSSAccessKeyId + ":" + Signature;
        return Signature;
    }



    #endregion

    #region private

    private static string HmacsHa1(string data, string key)
    {
        //Debug.Log(data);
        var algorithm = KeyedHashAlgorithm.Create("HmacSHA1".ToUpperInvariant());
        algorithm.Key = Encoding.UTF8.GetBytes(key.ToCharArray());
        var encode = Encoding.UTF8.GetBytes(data.ToCharArray());
        var alg = algorithm.ComputeHash(encode);
        //Debug.Log(Convert.ToBase64String(alg));
        return Convert.ToBase64String(alg);
    }

    private static string urlencode(string url)
    {
        return WWW.EscapeURL(url);
    }


    private static string GetTime()
    {
        const long ticksOf1970 = 621355968000000000;
        return ((DateTime.Now.AddMinutes(60).ToUniversalTime().Ticks - ticksOf1970) / 10000000L)
            .ToString(CultureInfo.InvariantCulture);
    }
    #endregion

}
