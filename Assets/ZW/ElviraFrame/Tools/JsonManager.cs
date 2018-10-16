/*******
* Copyright (C)2018    Administrator 
* 创建人:              Administrator  
* 创建时间:            2018/6/15 星期五 15:46:39    
****************************/
using System;
using System.Collections.Generic;
using  System.IO;
using  UnityEngine;

namespace ElviraFrame { 
public  class JsonManager
{
    public static PointClass ReadJsonFromFile()
    {

        if (!File.Exists(Application.dataPath+ "/ZW/TestJson.json"))
        {
            return null;
        }
        StreamReader  sr=new StreamReader(Application.dataPath+ "/ZW/TestJson.json");

        if (sr==null)
        {
            return null;
        }

        string json = sr.ReadToEnd();
        if (json.Length>0)
        {
            return JsonUtility.FromJson<PointClass>(json);
        }

        return null;
    }
    public static string ReadJsonFromFilePath(string  filePath,string fileName)
    {

            //1.找到资源保存的文件夹
       //     string assetDirectory = UnityHelper.LocalFilePath + "/Web/"+ fileName;
            string assetDirectory = filePath  + fileName;
            StreamReader sr = new StreamReader(@assetDirectory);

            if (sr == null)
        {
            return null;
        }

        string json = sr.ReadToEnd();
        if (json.Length > 0)
            {
                sr.Close();
                sr.Dispose();
                return json;
        }
            sr.Close();
            sr.Dispose();
            return null;
    }
    public static void SaveJsonToFile(PointClass status)
    {
          string json=   JsonUtility.ToJson(status,true);
            string savePath = PublicAttribute.LocalFilePath + "/Web/PointMap.json";
            FileStream aFile = new FileStream(@savePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
        sw.Write(json);
        sw.Close();
        sw.Dispose();


    }
        }
}
 
