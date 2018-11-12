using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Text;
using UnityEngine.UI;
using ElviraFrame;
using UnityEngine.SceneManagement;


public class CopyFileSystem : SingletonMono<CopyFileSystem>
{


    public override void Awake()
    {
        base.Awake();

    CreateFileDirectory(PublicAttribute.LocalFilePath + "Push/"+PublicAttribute.AreaId+"/a.txt");
        CreateFileDirectory(PublicAttribute.LocalFilePath + "Web/PointMap.json");
        CreateFileDirectory("/sdcard/DCIM/AR游/a.txt");
        CopyFile();
        GetPushResources();
    }

    public void GetPushResources()
    {
        string contentName = "content.json";
        string filePath = PublicAttribute.LocalFilePath + "Push/" + PublicAttribute.AreaId + "/";
        if (!File.Exists(filePath+ contentName))
        {
            HttpManager.Instance.GetPushContent((b,name)=>
        {
            if (b)
            {
                StartCoroutine(DownLoadContent(name));
            
            }
        });
        }
        else
        {
            CreateFileDirectory(filePath + "a.txt");
            HttpManager.Instance.GetPushContent((b, name) =>
            {
                if (b)
                {
                    StartCoroutine(DownLoadContent(name));

                }
            });
        }
    }

 IEnumerator DownLoadContent(string  name)
    {

        Debug.Log("dfgsdgdfsgfdytrutyikgfhdf");
        WWW wW = new WWW("http://vsz-scenery-area.vszapp.com/" + name + ".json");

        yield return wW;
        if (wW.isDone&&wW.error==null)
        {
            File.WriteAllText(PublicAttribute.LocalFilePath+ "Push/" + PublicAttribute.AreaId + "/content.json", wW.text);
        }
    PushManager.Instance.LoadPushJson();
    }

    void CreateFileDirectory(string filePath)
    {
        if (!File.Exists(filePath))
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Debug.Log("CreateDirectary");
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);

            sw.Close();
            sw.Dispose();
            fs.Close();
        }

    }

    private void CopyFile()
    {
        string htmlDirectoryPath = UnityHelper.LocalFilePath + "Web/";
        string pushDirectoryPath = UnityHelper.LocalFilePath + "Push/";

#if UNITY_ANDROID
        StartCoroutine(Load(htmlDirectoryPath + "CustomOverlay.html", "CustomOverlay.html", "Web"));
        StartCoroutine(Load(htmlDirectoryPath + "jquery-1.12.2.min.js", "jquery-1.12.2.min.txt", "Web"));
  //      StartCoroutine(Load(htmlDirectoryPath + "PointMap.json", "PointMap.json", "Web"));


        //       WebView.Instance.CreateWebView();
        string htmlsourceFile = Application.streamingAssetsPath + "/Web/CustomOverlay.html";
        //   File.Copy(htmlDirectoryPath + "CustomOverlay.html", htmlDirectoryPath + "CustomOverlay.html", true);
#else
 
   
        File.Copy(Application.streamingAssetsPath + "/Web/CustomOverlay.html",PublicAttribute.LocalFilePath + "Web/CustomOverlay.html",true);
        File.Copy(Application.streamingAssetsPath + "/Web/jquery-1.12.2.min.txt", PublicAttribute.LocalFilePath + "Web/jquery-1.12.2.min.js", true);
        File.Copy(Application.streamingAssetsPath + "/Web/PointMap.json", PublicAttribute.LocalFilePath + "Web/PointMap.json", true);
             //File.Copy(pushjsonsourceFile,pushjsontargetFile,true);
#endif


    }

    /// <summary>
    /// 加载streamingAssets目录下的内容并且写到本地文件夹中去
    /// </summary>
    /// <param name="targetFile">   完整的文件路径名字</param>
    /// <param name="fileName">文件自身的名字</param>
    /// <param name="fileparentName">文件父亲的名字</param>
    /// <returns></returns>
    IEnumerator Load(string targetFile, string fileName, string fileparentName)
    {

        WWW load = new WWW("jar:file://" + Application.dataPath + "!/assets/" + fileparentName + "/" + fileName);  // 安卓下streamingAssets目录

        yield return load;
        if (load.isDone)
        {
            // 写到持久化目录
            //    Debug.Log("load.text==" + load.text);
            File.WriteAllText(targetFile, load.text, System.Text.Encoding.UTF8);
        }
    }



}