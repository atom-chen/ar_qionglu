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
        CopyFile();
    }
    private void CopyFile()
    {
        string htmlPath = UnityHelper.LocalFilePath + "Web/a.txt";
        string pushcontentPath = UnityHelper.LocalFilePath + "Push/a.txt";


        string htmltargetFile = UnityHelper.LocalFilePath + "Web/" + "CustomOverlay.html";



        if (!File.Exists(htmlPath))
        {
            if (!Directory.Exists(Path.GetDirectoryName(htmlPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(htmlPath));
            }
     
            FileStream fs = new FileStream(htmlPath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();

        }


        string pushjsontargetFile = UnityHelper.LocalFilePath + "Push/" + "content.json";
        if (!File.Exists(pushcontentPath))
        {
            if (!Directory.Exists(Path.GetDirectoryName(pushcontentPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(pushcontentPath));
            }

            FileStream fs = new FileStream(pushcontentPath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Close();

        }



#if UNITY_ANDROID
        StartCoroutine(Load(htmltargetFile, "CustomOverlay.html","Web"));
        StartCoroutine(Load(pushjsontargetFile, "content.json","Push"));
#else
        string htmlsourceFile = Application.streamingAssetsPath + "/Web/CustomOverlay.html";
             string pushjsonsourceFile = Application.streamingAssetsPath + "/Push/content.json";
   
        File.Copy(htmlsourceFile,htmltargetFile,true);
             File.Copy(pushjsonsourceFile,pushjsontargetFile,true);
#endif
    }

    IEnumerator Load(string targetFile, string fileName,string  fileparentName)
    {

        WWW load = new WWW("jar:file://" + Application.dataPath + "!/assets/"+fileparentName+"/" + fileName);  // 安卓下streamingAssets目录

        yield return load;
        if (load.isDone)
        {
            // 写到持久化目录
            Debug.Log("load.text==" + load.text);
            File.WriteAllText(targetFile, load.text);
        }
    }
}
