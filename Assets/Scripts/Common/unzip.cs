using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;

public class unzip : MonoBehaviour
{
    public static unzip _inst;
    private void Awake()
    {
        _inst = this;
    }

    // Use this for initialization
	void Start ()
	{
	    // StartCoroutine(Wait_LoadDown("DownloadFile", Application.streamingAssetsPath + "/ios.zip", (b) =>
	    // {
	    //     Debug.Log("aaaaaaaaaaaaaaaaaa");
	    // }));
	}

    public void unzipres(Action<bool> callback)
    {
        StartCoroutine(Wait_LoadDown("DownloadFile", Application.streamingAssetsPath + "/ios.zip",(b =>
        {
            if (b)
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        })));
    }
    
    IEnumerator Wait_LoadDown(string ZipID,string url,Action<bool> calback)
    {
#if UNITY_EDITOR || UNITY_IOS || UNITY_IPHONE
        url = "file://" + url;
#elif UNITY_ANDROID
        url=url;
#endif
        WWW www = new WWW(url);
        yield return www;
        if (www.isDone)
        {
            if (www.error == null)
            {
                Debug.Log("下载成功");
                string dir = Application.persistentDataPath;
                Debug.Log(dir);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                yield return new WaitForEndOfFrame();
                //直接使用 将byte转换为Stream，省去先保存到本地在解压的过程
                SaveZip(ZipID, www.bytes,null ,(b) =>
                {
                    if (b)
                    {
                        if (calback!=null)
                        {
                            calback(true);
                        }
                        Debug.Log("解压成功");
                        // //1.找到资源保存的文件夹
                        // string assetDirectory = Application.persistentDataPath + "/DownloadFile/Panorama/";
                        // //Debug.Log(assetDirectory);
                        //
                        // DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);
                        // DirectoryInfo[] ScenicDirectories = directoryInfo.GetDirectories();
                        // //2.遍历里面的每个场景文件夹
                        // foreach (DirectoryInfo tmpDirectoryInfo in ScenicDirectories)
                        // {
                        //     string ScenicDirectory = assetDirectory + "/" + tmpDirectoryInfo.Name;
                        //     DirectoryInfo ScenicDirectoryInfo = new DirectoryInfo(ScenicDirectory);
                        //     DirectoryInfo[] ScenicPointDirectories = ScenicDirectoryInfo.GetDirectories();
                        //
                        //     foreach (DirectoryInfo DirectoryInfo in ScenicPointDirectories)
                        //     {
                        //         string ScenicPointDirectory = ScenicDirectory + "/" + DirectoryInfo.Name;
                        //         DirectoryInfo ScenicPointDirectoryInfo = new DirectoryInfo(ScenicPointDirectory);
                        //
                        //         //错误检测
                        //         if (ScenicPointDirectoryInfo == null)
                        //         {
                        //             Debug.LogError(ScenicDirectory + " 不存在!");
                        //             return;
                        //         }
                        //         else
                        //         {
                        //             Dictionary<string, string> namePahtDict = new Dictionary<string, string>();
                        //
                        //             //3.遍历场景文件夹里的所有文件系统
                        //             int index = ScenicPointDirectory.LastIndexOf("/");
                        //             Debug.Log(ScenicPointDirectory);
                        //             string sceneName = ScenicPointDirectory.Substring(index + 1);
                        //             Debug.Log(sceneName);
                        //         }
                        //     }
                        //
                        // }
                    }
                    else
                    {
                        Debug.Log("解压失败");
                        if (calback!=null)
                        {
                            calback(false);
                        }
                    }
                });

            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
    
    /// <summary> 
    /// 解压功能(下载后直接解压压缩文件到指定目录) 
    /// </summary> 
    /// <param name="wwwStream">www下载转换而来的Stream</param> 
    /// <param name="zipedFolder">指定解压目标目录(每一个Obj对应一个Folder)</param> 
    /// <param name="password">密码</param> 
    public static void SaveZip(string ZipID,byte[] ZipByte, string password, Action<bool> callback)
    {
        
            bool result = true;
            FileStream fs = null;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;

            ZipID = PublicAttribute.LocalFilePath;
            Debug.Log(ZipID);

            if (!Directory.Exists(ZipID))
            {
                Directory.CreateDirectory(ZipID);
            }
            try
            {
                //直接使用 将byte转换为Stream，省去先保存到本地在解压的过程
                Stream stream = new MemoryStream(ZipByte);
                zipStream = new ZipInputStream(stream);

                if (!string.IsNullOrEmpty(password))
                {
                    zipStream.Password = password;
                }
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        fileName = Path.Combine(ZipID, ent.Name);

                        #region      Android
                        fileName = fileName.Replace('\\', '/');

                        if (fileName.EndsWith("/"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }
                        #endregion
                        fs = File.Create(fileName);

                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                //fs.Write(data, 0, data.Length);
                                fs.Write(data, 0, size);//解决读取不完整情况
                            }
                            else
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        callback(result);
    }
}
