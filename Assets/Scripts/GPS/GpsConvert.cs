using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;

public class GpsPoint
{
    public string name; //点位名称
    public double lng; //经度
    public double lat; //纬度
    public double height; //海拔
}

public class GpsConvert : MonoBehaviour
{
    public static GpsConvert instance;
    [SerializeField]
    private GPSItem obj;

    private GpsPoint CamGPS = new GpsPoint();
    #region  GPS
    // Lat Lon to UTM variables
    // equatorial radius
    double equatorialRadius = 6378137;

    // polar radius
    double polarRadius = 6356752.314;

    // flattening
    double flattening = 0.00335281066474748;// (equatorialRadius-polarRadius)/equatorialRadius;

    // inverse flattening 1/flattening
    double inverseFlattening = 298.257223563;// 1/flattening;

    // Mean radius
    double rm;

    // scale factor
    double k0 = 0.9996;

    // eccentricity
    double e;

    double e1sq;

    double n;

    // r curv 1
    double rho = 6368573.744;

    // r curv 2
    double nu = 6389236.914;

    // Calculate Meridional Arc Length
    // Meridional Arc
    double S = 5103266.421;

    double A0 = 6367449.146;

    double B0 = 16038.42955;

    double C0 = 16.83261333;

    double D0 = 0.021984404;

    double E0 = 0.000312705;

    // Calculation Constants
    // Delta Long
    double p = -0.483084;

    double sin1 = 4.84814E-06;

    // Coefficients for UTM Coordinates
    double K1 = 5101225.115;

    double K2 = 3750.291596;

    double K3 = 1.397608151;

    double K4 = 214839.3105;

    double K5 = -2.995382942;

    double A6 = -1.00541E-07;




    #endregion
    private List<GpsPoint> gpsPoints = new List<GpsPoint>();
    [SerializeField]
    public List<GPSItem> GPSItems = new List<GPSItem>();

    private double x;
    private double z;
    private DateTime lastDateTime;

    public static double lat = 0, lng = 0, height = 0;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
        n = (equatorialRadius - polarRadius) / (equatorialRadius + polarRadius);
        rm = POW(equatorialRadius * polarRadius, 1 / 2.0);
        e = Math.Sqrt(1 - POW(polarRadius / equatorialRadius, 2));
        e1sq = e * e / (1 - e * e);
        getLocation();
    }
    void Start()
    {
        GetPoint();
        StartCoroutine(StartGPS());
    }
    private void StopGPS()
    {
        Input.location.Stop();
    }
    private IEnumerator StartGPS()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS");
            yield break;
        }

        // LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用
        Input.location.Start(1f, 1f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Init GPS service time out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("无法确定设备位置");
            yield break;
        }
        else
        {
            // lat = Input.location.lastData.latitude;
            // lng = Input.location.lastData.longitude;
            // height = Input.location.lastData.altitude;

            //  SetCamGpsPoint(Input.location.lastData.longitude, Input.location.lastData.latitude, Input.location.lastData.altitude, "cam");


            lat = getLocation().y;
            lng = getLocation().x;
            height = getLocation().z;

            SetCamGpsPoint(lng, lat, height, "cam");
            yield return new WaitForSeconds(100);
        }
    }
    private void RefreshGPS()
    {
        lat = getLocation().y;
        lng = getLocation().x;
        height = getLocation().z;

        SetCamGpsPoint(lng, lat, height, "cam");
    }
    public Vector3 getLocation()
    {
#if UNITY_EDITOR
        return new Vector3(104.0597f, 30.59437f, 576.7651f);
#elif UNITY_ANDROID

        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        String location = jo.Call<String>("getLocation");
        debugLog(location);

        Debug.Log("GPS::::::::::" + location);


        JsonData zb = JsonMapper.ToObject(location);
        float x = float.Parse(zb["longitude"].ToString());
        float y =float.Parse(zb["latitude"].ToString());
        float z = float.Parse(zb["altitude"].ToString());
        return new Vector3(x,y,z);


#elif UNITY_IOS||UNITY_IPHONE
        return new Vector3(104.0597f, 30.59437f, 576.7651f);
#endif
    }

    public void debugLog(String msg)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("logTest", msg);
    }
    private void OnDisable()
    {
        StopGPS();
    }
    public void SetCamGpsPoint(double lng, double lat, double height, string name)
    {
        for (int i = 0; i < icon.Count; i++)
        {
            Destroy(icon[i]);
        }
        icon.Clear();
        CamGPS.name = name;
        CamGPS.lng = lng;
        CamGPS.lat = lat;
        CamGPS.height = height;
        CreatePoints("all");
    }
    void GetPoint()
    {
        foreach (var GPSinfo in mainPageUI.curScenicInfo.ResourcesInfos)
        {
            if (GPSinfo.ResourcesKey == "scenery_guide")
                if (GPSinfo.DIS.Count <= 0)
                {
                    Invoke("GetPoint", 1);
                }
                else
                {
                    foreach (var fileitem in GPSinfo.DIS)
                    {
                        Debug.Log(fileitem.typeName);
                        GPSItem item = GameObject.Instantiate<GPSItem>(obj);
                        item.address = fileitem.baseEntity.address;
                        item.locationX = fileitem.baseEntity.locationX;
                        item.locationY = fileitem.baseEntity.locationY;
                        item.thumbnail = fileitem.baseEntity.thumbnail;
                        item.id = fileitem.baseEntity.id;
                        item.name = fileitem.baseEntity.name;
                        item.height = fileitem.baseEntity.height;
                        item.sceneryArea = fileitem.baseEntity.sceneryArea;
                        item.content = fileitem.baseEntity.content;
                        item.typeName = fileitem.typeName;
                        GPSItems.Add(item);
                    }
                }
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetCamGpsPoint(102.278882, 27.842102, 576.7651, "cam");
        }
    }
    public static List<GameObject> icon = new List<GameObject>();
    private void CreatePoints(string typename)
    {
        double camx, camz, x, z;
        Vector3 objPos;
        GpsConvertXZ(CamGPS.lng, CamGPS.lat, out camx, out camz);
        foreach (GPSItem item in GPSItems)
        {
            if (typename == "all" || typename == item.typeName)
            {
                if (item.locationX.Length < 2 || item.locationY.Length < 2)
                {
                    Debug.Log("没坐标");
                }
                else
                {
                    GpsConvertXZ(float.Parse(item.locationX), float.Parse(item.locationY), out x, out z);
                    objPos = new Vector3((float)(x - camx), (float)(float.Parse(item.height) - CamGPS.height),
                        (float)(z - camz));
                    item.gameObject.SetActive(true);
                    item.transform.position = objPos;
                    //info.text += "distance：" + (int)Vector3.Distance(Camera.main.transform.position, newGO.transform.position) + "\n";
                    item.gameObject.AddComponent<IconFollow>();
                    item.icon = item.GetComponent<IconFollow>();
                }
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private bool IsShowDirectSpot;
    public void ShowDirectSpot(string id)
    {
        foreach (GPSItem item in GPSItems)
        {
            if (item.id == id)
            {
                IsShowDirectSpot = true;
                if (item.icon != null)
                    item.icon.ShowDirectSpot();
            }
            else
            {
                //Debug.Log(item.id);
                if (item.icon != null)
                    item.icon.ChangeFade(0.3f);
            }
        }
    }
    public void ChangeDropDown(Dropdown Drop)
    {
        Debug.Log(Drop.value);
        switch (Drop.value)
        {
            case 0:
                CreatePoints("all");
                break;
            case 1:
                CreatePoints("景区");
                break;
            case 2:
                CreatePoints("特产");
                break;
            case 3:
                CreatePoints("商家");
                break;
        }
    }
    public void BackMain()
    {
        if (IsShowDirectSpot)
        {
            IsShowDirectSpot = false;
            foreach (GPSItem item in GPSItems)
            {
                if (item.icon != null)
                    item.icon.ChangeFade(1f);
            }
        }
        else
        {
            SceneManager.LoadScene("main");
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lng">经度</param>
    /// <param name="lat">纬度</param>
    /// <param name="x">输出x坐标</param>
    /// <param name="z">输出y坐标</param>
    void GpsConvertXZ(double lng, double lat, out double x, out double z)
    {
        if (lat > 85.05112)
        {
            lat = 85.05112;
        }

        if (lat < -85.05112)
        {
            lat = -85.05112;
        }

        lat = (Math.PI / 180.0) * lat;
        var tmp = Math.PI / 4.0 + lat / 2.0;

        z = 20037508.34 * Math.Log(Math.Tan(tmp)) / Math.PI;
        x = (lng / 180.0) * 20037508.34;
    }

#region GPS坐标计算
    public String convertLatLonToUTM(double latitude, double longitude)
    {
        validate(latitude, longitude);
        String UTM = "";
        setVariables(latitude, longitude);

        String longZone = getLongZone(longitude);
        LatZones latZones = new LatZones();
        String latZone = latZones.getLatZone(latitude);

        double _easting = getEasting();
        double _northing = getNorthing(latitude);

        UTM = longZone + " " + latZone + " " + ((int)_easting) + " "
            + ((int)_northing);
        // UTM = longZone + " " + latZone + " " + decimalFormat.format(_easting) +
        // " "+ decimalFormat.format(_northing);

        return UTM;

    }
    protected void setVariables(double latitude, double longitude)
    {
        latitude = DegreeToRadian(latitude);
        rho = equatorialRadius * (1 - e * e)
            / POW(1 - POW(e * SIN(latitude), 2), 3 / 2.0);

        nu = equatorialRadius / POW(1 - POW(e * SIN(latitude), 2), (1 / 2.0));

        double var1;
        if (longitude < 0.0)
        {
            var1 = ((int)((180 + longitude) / 6.0)) + 1;
        }
        else
        {
            var1 = ((int)(longitude / 6)) + 31;
        }
        double var2 = (6 * var1) - 183;
        double var3 = longitude - var2;
        p = var3 * 3600 / 10000;

        S = A0 * latitude - B0 * SIN(2 * latitude) + C0 * SIN(4 * latitude) - D0
            * SIN(6 * latitude) + E0 * SIN(8 * latitude);

        K1 = S * k0;
        K2 = nu * SIN(latitude) * COS(latitude) * POW(sin1, 2) * k0 * (100000000)
            / 2;
        K3 = ((POW(sin1, 4) * nu * SIN(latitude) * POW(COS(latitude), 3)) / 24)
            * (5 - POW(TAN(latitude), 2) + 9 * e1sq * POW(COS(latitude), 2) + 4
                * POW(e1sq, 2) * POW(COS(latitude), 4))
            * k0
            * (10000000000000000L);

        K4 = nu * COS(latitude) * sin1 * k0 * 10000;

        K5 = POW(sin1 * COS(latitude), 3) * (nu / 6)
            * (1 - POW(TAN(latitude), 2) + e1sq * POW(COS(latitude), 2)) * k0
            * 1000000000000L;

        A6 = (POW(p * sin1, 6) * nu * SIN(latitude) * POW(COS(latitude), 5) / 720)
            * (61 - 58 * POW(TAN(latitude), 2) + POW(TAN(latitude), 4) + 270
                * e1sq * POW(COS(latitude), 2) - 330 * e1sq
                * POW(SIN(latitude), 2)) * k0 * (1E+24);

    }
    protected String getLongZone(double longitude)
    {
        double longZone = 0;
        if (longitude < 0.0)
        {
            longZone = ((180.0 + longitude) / 6) + 1;
        }
        else
        {
            longZone = (longitude / 6) + 31;
        }
        String val = ((int)longZone).ToString();
        if (val.Length == 1)
        {
            val = "0" + val;
        }
        return val;
    }
    protected double getNorthing(double latitude)
    {
        double northing = K1 + K2 * p * p + K3 * POW(p, 4);
        if (latitude < 0.0)
        {
            northing = 10000000 + northing;
        }
        return northing;
    }
    protected double getEasting()
    {
        return 500000 + (K4 * p + K5 * POW(p, 3));
    }
    public double DegreeToRadian(double degree)
    {
        return degree * Math.PI / 180;
    }
    private void validate(double latitude, double longitude)
    {
        if (latitude < -90.0 || latitude > 90.0 || longitude < -180.0 || longitude >= 180.0)
        {
            throw new Exception("Legal ranges: latitude [-90,90], longitude [-180,180).");
        }
    }
    private double POW(double a, double b)
    {
        return Math.Pow(a, b);
    }
    private double SIN(double value)
    {
        return Math.Sin(value);
    }
    private double COS(double value)
    {
        return Math.Cos(value);
    }
    private double TAN(double value)
    {
        return Math.Tan(value);
    }
#endregion

#region 获取时间戳

    public string getTime(string _time)
    {
        string timeStamp = _time;
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime dtResult = dtStart.Add(toNow);
        Debug.Log(dtResult);
        string date = dtResult.ToShortDateString().ToString();
        string time = dtResult.ToLongTimeString().ToString();
        string[] date_arr = date.Split('/');
        string[] time_arr = time.Split(':');
        string result = date_arr[0] + "月" + date_arr[1] + "日" + " " + time_arr[0] + "时" + time_arr[1] + "分";
        return result;
    }

    /// <summary>        
    /// 时间戳转为C#格式时间        
    /// </summary>        
    /// <param name=”timeStamp”></param>        
    /// <returns></returns>        
    public string ConvertStringToDateTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000");
        TimeSpan toNow = new TimeSpan(lTime);

        DateTime dtResult = dtStart.Add(toNow);
        dtStart.Add(toNow);
        lastDateTime = dtResult;

        // dtResult.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        return dtResult.ToString("MM-dd HH:mm:ss:ff") + " 间隔: " + DateDiff(lastDateTime, dtResult);
    }


    /// <summary>
    /// 计算两个日期的时间间隔
    /// </summary>
    /// <param name="DateTime1">第一个日期和时间</param>
    /// <param name="DateTime2">第二个日期和时间</param>
    /// <returns></returns>
    private string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {

        string dateDiff = null;

        TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
        TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();
        //dateDiff = ts.Days.ToString() + "天"+ ts.Hours.ToString() + "小时"+ ts.Minutes.ToString() + "分钟"+ ts.Seconds.ToString() + "秒";
        dateDiff = ts.Minutes.ToString() + "分" + ts.Seconds.ToString() + "秒";
        lastDateTime = DateTime2;
        return dateDiff;
    }
#endregion
}
public class LatZones
{
    private char[] letters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
        'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Z' };

    private int[] degrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24, -16,
        -8, 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

    private char[] negLetters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
        'L', 'M' };

    private int[] negDegrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24,
        -16, -8 };

    private char[] posLetters = { 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
        'X', 'Z' };

    private int[] posDegrees = { 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

    private int arrayLength = 22;

    public LatZones()
    {
    }

    public int getLatZoneDegree(String letter)
    {
        char ltr = letter[0];
        for (int i = 0; i < arrayLength; i++)
        {
            if (letters[i] == ltr)
            {
                return degrees[i];
            }
        }
        return -100;
    }

    public String getLatZone(double latitude)
    {
        int latIndex = -2;
        int lat = (int)latitude;

        if (lat >= 0)
        {
            int len = posLetters.Length;
            for (int i = 0; i < len; i++)
            {
                if (lat == posDegrees[i])
                {
                    latIndex = i;
                    break;
                }

                if (lat > posDegrees[i])
                {
                    continue;
                }
                else
                {
                    latIndex = i - 1;
                    break;
                }
            }
        }
        else
        {
            int len = negLetters.Length;
            for (int i = 0; i < len; i++)
            {
                if (lat == negDegrees[i])
                {
                    latIndex = i;
                    break;
                }

                if (lat < negDegrees[i])
                {
                    latIndex = i - 1;
                    break;
                }
                else
                {
                    continue;
                }

            }

        }

        if (latIndex == -1)
        {
            latIndex = 0;
        }
        if (lat >= 0)
        {
            if (latIndex == -2)
            {
                latIndex = posLetters.Length - 1;
            }
            return posLetters[latIndex].ToString();
        }
        else
        {
            if (latIndex == -2)
            {
                latIndex = negLetters.Length - 1;
            }
            return negLetters[latIndex].ToString();

        }
    }
}
