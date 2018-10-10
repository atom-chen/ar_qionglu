using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System.Text;
using LitJson;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// GPS管理类；
/// 
/// latitude：纬度；
/// 
/// 
/// longitude：经度
/// </summary>
public class GPSManager : SingletonMono<GPSManager>
{

    float[] data=new float[2];
    /// <summary>
    /// 返回GPS值数组，[0]是经度，[1]是纬度
    /// </summary>
    /// <returns></returns>
    public float[] GetGPS()
    {
        StartCoroutine(GetGPSLocation());
    
     
        return data;
    }

    IEnumerator GetGPSLocation()
    {
    

        // 检查位置服务是否可用  
        if (!Input.location.isEnabledByUser)
        {
       
            yield break;
        }

        // 查询位置之前先开启位置服务  
        Input.location.Start();

        // 等待服务初始化  
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // 服务初始化超时  
        if (maxWait < 1)
        {
            yield break;
        }

        // 连接失败  
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }
        else
        {
            float[] olddata = new float[2] { Input.location.lastData.longitude, Input.location.lastData.latitude };
            float[] newdata = ChangeZuoBiao(olddata);
            data[0]= newdata[0];
            data[1] = newdata[1];
        }

        // 停止服务，如果没必要继续更新位置，（为了省电）  
        Input.location.Stop();
    }

    public float[] ChangeZuoBiao(float[]  data )
    {
        float[] datas = new float[2];
        //获取当前纬度
        string Latitude = data[1].ToString("F8");

        //获取当前经度
        string Longitude = data[0].ToString("F8");
        //百度坐标转换API
      //  http://api.map.baidu.com/geoconv/v1/?coords=114.21892734521,29.575429778924&from=1&to=5&ak=你的密钥
        string path = "http://api.map.baidu.com/geoconv/v1/?coords=" + Latitude + "," + Longitude + "&from=1&to=5&ak="+GlobalParameter.ak;//
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(path);
        webRequest.Method = "GET";
        HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
        StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
        string strJson = sr.ReadToEnd();
        Debug.Log(strJson);
        JsonData temp = JsonMapper.ToObject(strJson);
        JsonData result = temp["result"];
        

        //X:经度。Y：纬度
        datas[0]= float.Parse(result[0]["x"].ToString());
        datas[1] = float.Parse(result[0]["y"].ToString());

        return datas;

    }


    [DllImport("__Internal")]
    private static extern string GetLocation();//接收字符串



    /// <summary>
    /// 获取GPS经纬度信息
    /// 返回值
    /// {"altitude":460.1,"available":true,"latitude":30.597598,"longitude":104.066928}
    /// </summary>
    /// <returns></returns>
    public string GetBDGPSLocation()
    {
#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        String location = jo.Call<String>("getLocation");
        return location;
#endif
        return GetLocation();

    }

}