using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LitJson;

public class ChangeItem : MonoBehaviour {
    [DllImport("__Internal")]
    private static extern string GetLocation();//测试接收字符串
    public RawImage png;

    public Text title,info;
    public Vector2 GPS;
    /// <summary>
    /// 全景点详情
    /// </summary>
    public string content;
    /// <summary>
    /// 全景点ID
    /// </summary>
    public string id;
    /// <summary>
    /// 全景点名称
    /// </summary>
    public string name;
    /// <summary>
    /// 全景点缩略图信息
    /// </summary>
    public Thumbnail thumbnail;
    /// <summary>
    /// 全景点经度lon
    /// </summary>
    public string locationX;
    /// <summary>
    /// 全景点纬度lat
    /// </summary>
    public string locationY;
    /// <summary>
    /// 全景点海拔
    /// </summary>
    public string height;
    /// <summary>
    /// 所属的景区
    /// </summary>
    public SceneryArea sceneryArea;
    /// <summary>
    /// 网页详情地址
    /// </summary>
    public string address;
    /// <summary>
    /// 所有的资源
    /// </summary>
    public List<VersionFilesItem> VersionFilesItems;

    public string LocalAddress;
    webrequest web;

    private string VideoURL;


    public GameObject loadingImg;
    public void Start()
    {
        web = GameObject.Find(GlobalInfo.websiterequest).GetComponent<webrequest>();
        info.text = content;
        title.text = name;
        LocalAddress = thumbnail.localPath;
        _init(thumbnail.localPath);
    }
    // Use this for initialization
    public void _init(string assetpath)
    {
        info.text = content;
        title.text = name;
        StartCoroutine(LoadImgFromCache(assetpath, png));
    }

    private IEnumerator LoadImgFromCache(string imgURl, RawImage img)
    {
        if (CheckCacheUrlIsExit(imgURl))
        {
            Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            img.texture = tex;
            HttpBase.Download(imgURl, ((request, downloaded, length) => { }), ((request, response)
                =>
            {
                if (response == null || !response.IsSuccess)
                {
                    DebugManager.Instance.LogError("请求失败！");
                    return;
                }
                tex.LoadImage(response.Data);
            }));
        }
        else
        {
            yield break;
        }
    }
    /// <summary>
    /// 检查文件是否存在
    /// </summary>
    /// <param name="imgURl"></param>
    /// <returns></returns>
    private bool CheckCacheUrlIsExit(string imgURl)
    {
        return true;
    }

    public void BtnOnClick(GameObject obj)
    {
        Vector3 point = getLocation();
        GPS = new Vector2(point.x,point.y);
        
        Debug.Log("当前距离：：："+ GlobalInfo.Distance(GPS.y,GPS.x, float.Parse(locationY), float.Parse(locationX))*1000);
        if (GlobalInfo.Distance(GPS.y,GPS.x, float.Parse(locationY), float.Parse(locationX)) * 1000 < 40)
        {
            foreach (var item in VersionFilesItems.Where(item => item.extName == "mp4"))
            {
                Debug.Log(item.filename);
                if (item.filename.Contains("qionghaixingcheng"))
                {
                    GlobalInfo.VideoURL2D  = PublicAttribute.LocalFilePath + "/Panorama/1/"+item.filename;
                }
                else
                {
                    VideoURL  = PublicAttribute.LocalFilePath + "/Panorama/1/"+item.filename;
                    Debug.Log(VideoURL);
                }
              
            }
            
            foreach (var item in VersionFilesItems.Where(item => item.extName == "vsz"))
            {
                Debug.Log(item.localPath);
                StartCoroutine(LoadAssets(item.localPath));
            }
        }
        else
        {
            obj.SetActive(true);
        } 
    }

    public void OpenWeb()
    {
        web.LoadWebSetTitle(address,name);
    }
    private IEnumerator LoadAssets(string path)
    {
        loadingImg.SetActive(true);
        if (VideoURL != null)
        {
            GlobalInfo.VideoURL360 = VideoURL;
        }

        int index1 = path.LastIndexOf("/");
        int index2 = path.LastIndexOf(".");
        string suffix = path.Substring(index1 + 1, index2 - index1 - 1);

        //string FileBrower= path.Substring(0, index1);
        //int index3 = FileBrower.LastIndexOf("/");
        //string FileBrowername = FileBrower.Substring(0, index3);

        //path = Application.persistentDataPath + "/DownloadFile/Panorama/" + FileBrowername+"/"+ suffix+".vsz";
        //path = Application.persistentDataPath + "/DownloadFile/Panorama/1/shajin.vsz";

        Debug.Log(File.Exists(path));
        WWW bundle = new WWW("file:///" + path);
        yield return bundle;
        if (bundle.error!=null  || bundle.size <= 0)
        {
            loadingImg.SetActive(false);
        }
        else
        {
            var data = bundle.assetBundle;
            SceneManager.LoadScene(suffix);               
        }
    }
    private IEnumerator LoadAssets2()
    {
        string path = Application.dataPath + "/DownloadFile/Panorama/1/qionghai.vsz";
        Debug.Log(path);
        Debug.Log(File.Exists(path));
        WWW bundle =new WWW("file:///" + path);
        yield return bundle;
        Debug.Log(bundle.bytesDownloaded);
        var data = bundle.assetBundle;
        SceneManager.LoadScene("qionghai");
    }
    
    public Vector3 getLocation()
    {
#if UNITY_ANDROID

        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        String location = jo.Call<String>("getLocation");

        Debug.Log("GPS::::::::::" + location);


        JsonData zb = JsonMapper.ToObject(location);
        float x = float.Parse(zb["longitude"].ToString());
        float y = float.Parse(zb["latitude"].ToString());
        float z = float.Parse(zb["altitude"].ToString());
        return new Vector3(x,y,z);
#elif UNITY_IOS || UNITY_IPHONE
      string IosGet = GetLocation();
      Debug.Log(IosGet);
    if (IosGet.Length < 5)
    {
         return new Vector3(0f, 0f, 0f);
    }
    else
    {
        JsonData zb = JsonMapper.ToObject(IosGet);
        float x = float.Parse(zb["longitude"].ToString());
        float y = float.Parse(zb["latitude"].ToString());
        float z = float.Parse(zb["altitude"].ToString());
        return new Vector3(x,y,z);
    }
#endif
    }
}
