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

    public static string ReadJsonFromFilePath(string  filePath,string fileName)
    {

            string assetDirectory = filePath  + fileName;
            if (!File.Exists(assetDirectory))
                return null;
            string json  = File.ReadAllText(assetDirectory);   
        if (json.Length > 0)
            {
                Debug.Log(json);
                return json;
        }     
            return null;
    }
    public static void SaveJsonToFile(PointClass status)
        {


            string savePath = UnityHelper.LocalFilePath + "Web/PointMap.json";
            if (!Directory.Exists(UnityHelper.LocalFilePath + "Web"))
            {
                Directory.CreateDirectory(UnityHelper.LocalFilePath + "Web");
            }

            string json=   JsonUtility.ToJson(status,true);
 

                Debug.Log("savePath======" + savePath);
            Debug.Log("json======" + json);
      File.WriteAllText(savePath, json);

    }
        }
}
 
