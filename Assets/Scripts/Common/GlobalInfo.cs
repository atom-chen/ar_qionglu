using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class GlobalInfo {
    //全景视频
    public static string VideoURL360;
    public static string VideoURL2D;
    
    public static float GPSId;
    public static string GPSType;
    public static bool GPSdirect;
    public static string websiterequest = "websiterequest";
    
    public static string LastAdsUrl
    {
        set {PlayerPrefs.SetString("LastAdsUrl", value);}
        get { return PlayerPrefs.GetString("LastAdsUrl");}
    }
    public static int ShotCount
    {
        set {PlayerPrefs.SetInt("ShotCount", value);}
        get { return PlayerPrefs.GetInt("ShotCount");}
    }
    public static string LastAdsImgPath
    {
        set {PlayerPrefs.SetString("LastAdsImgPath", value);}
        get { return PlayerPrefs.GetString("LastAdsImgPath");}
    }

    public static string APPversion = "1.02";
    public static string AppDownloadPage = "http://download.vszapp.com/";
    
    public static int requestTime()
    {
        int seconds = Convert.ToInt32(DateTime.UtcNow.Subtract(DateTime.Parse("1970-1-1")).TotalSeconds);
        return seconds + 1800;
    }
    public static string GetMD5(string str)
    {
        //创建 MD5对象
        MD5 md5 = MD5.Create();//new MD5();
        //开始加密
        //需要将字符串转换成字节数组
        byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
        
        //md5.ComputeHash(buffer);

        //返回一个加密好的字节数组
        byte[] MD5Buffer = md5.ComputeHash(buffer);

        //将字节数组 转换成字符串

        /*
        字节数组  --->字符串
         * 1、将字节数组中的每个元素按照指定的编码格式解析成字符串
         * 2、直接ToString()
         * 3、将字节数组中的每个元素都ToString()
         */
        //return Encoding.GetEncoding("GBK").GetString(MD5Buffer);

        string strNew = "";
        for (int i = 0; i < MD5Buffer.Length; i++)
        {
            strNew += MD5Buffer[i].ToString("x2");
        }
        return strNew;
    }

    public static string stringSub(string str)
    {
        if (str.StartsWith("vsz-"))
        {
            return str.Substring(4);
        }
        else
        {
            return str;
        }
    }
    
    #region  GPS距离计算
    
    public static double HaverSin(double theta)
    {
        var v = Math.Sin(theta / 2);
        return v * v;
    }


    static double EARTH_RADIUS = 6371.0;//km 地球半径 平均值，千米

    /// <summary>
    /// 给定的经度1，纬度1；经度2，纬度2. 计算2个经纬度之间的距离。
    /// </summary>
    /// <param name="lat1">经度1</param>
    /// <param name="lon1">纬度1</param>
    /// <param name="lat2">经度2</param>
    /// <param name="lon2">纬度2</param>
    /// <returns>距离（公里、千米）</returns>
    public static double Distance(double lat1,double lon1, double lat2,double lon2)
    {
        //用haversine公式计算球面两点间的距离。
        //经纬度转换成弧度
        lat1 = ConvertDegreesToRadians(lat1);
        lon1 = ConvertDegreesToRadians(lon1);
        lat2 = ConvertDegreesToRadians(lat2);
        lon2 = ConvertDegreesToRadians(lon2);

        //差值
        var vLon = Math.Abs(lon1 - lon2);
        var vLat = Math.Abs(lat1 - lat2);

        //h is the great circle distance in radians, great circle就是一个球体上的切面，它的圆心即是球心的一个周长最大的圆。
        var h = HaverSin(vLat) + Math.Cos(lat1) * Math.Cos(lat2) * HaverSin(vLon);

        var distance = 2 * EARTH_RADIUS * Math.Asin(Math.Sqrt(h));

        return distance;
    }

    /// <summary>
    /// 将角度换算为弧度。
    /// </summary>
    /// <param name="degrees">角度</param>
    /// <returns>弧度</returns>
    public static double ConvertDegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    public static double ConvertRadiansToDegrees(double radian)
    {
        return radian * 180.0 / Math.PI;
    }
    
    #endregion
}
