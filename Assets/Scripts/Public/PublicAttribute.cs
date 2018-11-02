using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PublicAttribute
{
	

    /// <summary>
    /// app资源版本json文件
    /// </summary>
    public static string AppVersionJson = "App/AppVersionJson.json";

    /// <summary>
    /// app资源更新文件
    /// </summary>
    public static string AppVersionUpdateJson = "App/AppVersionUpdateJson.json";

    /// <summary>
    /// 当前app资源所有的文件
    /// </summary>
    public static string AppVersionFullJson = "App/AppVersionFullJson.json";

    /// <summary>
    /// 本地Token存放路径
    /// </summary>
    public static string TokenFilePath = LocalFilePath + "APP/Token.json";

    /// <summary>
    /// oss id
    /// </summary>
    public static string OSSAccessKeyId = "LTAIX9PZ52E9mVhE";

    /// <summary>
    /// oss 密钥
    /// </summary>
    public static string OSSAccessKeySecret = "wt7L6ioCEuCCHOAg7ZzlP8L9F7KrK8";

    /// <summary>
    /// OSS 链接
    /// </summary>
    //public static string OSSUri = ".oss-cn-beijing.aliyuncs.com/";
    public static string OSSUri = ".vszapp.com/";


    #region 缩略图本地存储路径
    private static string thumbPath ;
    /// <summary>
    /// 缩略图保存路径
    /// </summary>
    public static string ThumbPath
    {
        get
        {
            if (string.IsNullOrEmpty(thumbPath))
            {
                thumbPath = localFilePath + "Thumbnail/";
                if (!Directory.Exists(thumbPath))
                {
                    Directory.CreateDirectory(thumbPath);
                }
            }
            return thumbPath;
        }
        set { thumbPath = value; }
    }

    #endregion

    #region 广告类图片本地存储路径

    private static  string adsPath;
    /// <summary>
    ///  广告类图片本地存储路径
    /// </summary>
    public static string AdsPath
    {
        get
        {
            if (string.IsNullOrEmpty(adsPath))
            {
                adsPath = localFilePath + "Ads/";
                if (!Directory.Exists(adsPath))
                {
                    Directory.CreateDirectory(adsPath);
                }
            }
            return adsPath;
        }
        set { adsPath = value; }
    }


    #endregion

    /// <summary>
    /// 当前景区ID
    /// </summary>
    public static string AreaId = "1";

    private static string localFilePath;

    /// <summary>
    /// app文件存放路径
    /// </summary>
    public static string LocalFilePath
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor 
                ||Application.platform == RuntimePlatform.OSXEditor
                ||Application.platform == RuntimePlatform.OSXPlayer)
            {
                localFilePath = Application.dataPath + "/DownloadFile/";
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                localFilePath = Application.persistentDataPath + "/DownloadFile/";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                localFilePath = Application.persistentDataPath + "/DownloadFile/";
            }
            if (!Directory.Exists(localFilePath))
            {
                Directory.CreateDirectory(localFilePath);
            }
            return localFilePath;
        }
        set { localFilePath = value; }
    }

    private static string scenicSpotPagePath;

    /// <summary>
    /// 景点图片存放位置
    /// </summary>
    public static string ScenicSpotPagePath
    {
        get
        {
            scenicSpotPagePath = LocalFilePath + "ScenicSpotPage/";
            if (!Directory.Exists(scenicSpotPagePath))
            {
                Directory.CreateDirectory(scenicSpotPagePath);
            }
            return scenicSpotPagePath;
        }
        set { scenicSpotPagePath = value; }
    }

    public static UserInfo UserInfo;

    private static string platform;

    /// <summary>
    /// 当前平台
    /// </summary>
    public static string Platform
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                platform = "android";
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                platform = "android";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                platform = "ios";
            }
            return platform;
        }
        set { platform = value; }
    }

    private static string platformInt;

    /// <summary>
    /// 当前平台 数字
    /// </summary>
    public static string PlatformInt
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    platformInt = "1";
                    break;

                case RuntimePlatform.WindowsEditor:
                    platformInt = "1";
                    break;

                case RuntimePlatform.IPhonePlayer:
                    platformInt = "2";
                    break;
            }
            return platformInt;
        }
        set { platformInt = value; }
    }

    /// <summary>
    /// 公共访问URL   ;  "http://192.168.30.49:7070/"；"http://192.168.20.2:9090/";http://115.28.222.129:7070/   "http://ar.vszapp.com:7070/";
    /// </summary>
    public static string URL = "http://ar.vszapp.com:7070/";

    /// <summary>
    /// 经度
    /// </summary>
    public static double Longitude = 116.404268;

    /// <summary>
    /// 纬度
    /// </summary>
    public static double Latitude = 39.915598;

    /// <summary>
    /// 所有景区资源下载的Downloader的集合 用于控制批量的暂停
    /// </summary>
    public static Dictionary<string, Downloader> ResDownloaderDictionary = new Dictionary<string, Downloader>();

    /// <summary>
    /// app资源是否更新
    /// </summary>
    public static bool AppUpdateVersion = false;

    /// <summary>
    /// 用于分享的密钥
    /// </summary>
    private static string secreKey;

    /// <summary>
    /// 获取密钥
    /// </summary>
    /// <returns></returns>
    public static string GetSecretKey()
    {
        return secreKey;
    }

    /// <summary>
    /// 设置密钥
    /// </summary>
    /// <returns></returns>
    public static string SetSecretKey()
    {
        secreKey = Guid.NewGuid().ToString();
        return secreKey;
    }
   public static void SetUserInfo(string  userName,string tokenInfo)
    {
       UserName = userName;
      Token = tokenInfo;
    }
    private static string userName;
    public static string UserName
    {
        get {


            return userName;
        }
        set { userName = value; }
    }
    private static string token;

    /// <summary>
    /// Token验证码
    /// </summary>
    public static string Token
    {
        get
        {
            if (string.IsNullOrEmpty(token))
            {
                token = File.ReadAllText(TokenFilePath);
            }
            return token;
        }
        set
        {
            token = value;
            File.WriteAllText(TokenFilePath, value);
        }
    }

    #region 动态资源路径


    private static string visitPath;
    /// <summary>
    /// 到此一游文件路径
    /// </summary>
    public static string VisitPath
    {
        get
        {
            visitPath = LocalFilePath + "Visit/";
            if (!Directory.Exists(visitPath))
            {
                Directory.CreateDirectory(visitPath);
            }
            return visitPath;
        }
        set { visitPath = value; }
    }



    private static string panoramaPath;

    /// <summary>
    /// 全景图片存放位置
    /// 1、缩略图
    /// 2、AssetBundle资源包
    /// </summary>
    public static string PanoramaPath
    {
        get
        {
            panoramaPath = LocalFilePath + "Panorama/";
            if (!Directory.Exists(panoramaPath))
            {
                Directory.CreateDirectory(panoramaPath);
            }

            return panoramaPath;
        }
        set { panoramaPath = value; }
    }

    private static string navigationPath;

    /// <summary>
    /// 实景导览的存放位置
    /// </summary>
    public static string NavigationPath
    {
        get
        {
            navigationPath = LocalFilePath + "Navigation/";
            if (!Directory.Exists(navigationPath))
            {
                Directory.CreateDirectory(navigationPath);
            }
            return navigationPath;
        }
        set { navigationPath = value; }
    }

    private static string scan_ProductPath;

    /// <summary>
    /// AR土特产存放路径
    /// </summary>
    public static string Scan_ProductPath
    {
        get
        {
            scan_ProductPath = LocalFilePath + "scan_ProductPath/";
            if (!Directory.Exists(scan_ProductPath))
            {
                Directory.CreateDirectory(scan_ProductPath);
            }
            return scan_ProductPath;
        }
        set
        {
            scan_ProductPath = value;
        }
    }

    private static string scan_Ticket;

    /// <summary>
    /// AR门票
    /// </summary>
    public static string Scan_Ticket
    {
        get
        {
            scan_Ticket = LocalFilePath + "scan_Ticket/";
            if (!Directory.Exists(scan_Ticket))
            {
                Directory.CreateDirectory(scan_Ticket);
            }
            return scan_Ticket;
        }
        set { scan_Ticket = value; }
    }

    private static string scan_Conjure;

    /// <summary>
    /// AR召唤
    /// </summary>
    public static string Scan_Conjure
    {
        get
        {
            scan_Conjure = LocalFilePath + "scan_Conjure/";
            if (!Directory.Exists(scan_Conjure))
            {
                Directory.CreateDirectory(scan_Conjure);
            }
            return scan_Conjure;
        }
        set { scan_Conjure = value; }
    }

    private static string scan_More;

    /// <summary>
    /// AR扫一扫
    /// </summary>
    public static string Scan_More
    {
        get
        {
            scan_More = LocalFilePath + "scan_More/";
            if (!Directory.Exists(scan_More))
            {
                Directory.CreateDirectory(scan_More);
            }
            return scan_More;
        }
        set { scan_More = value; }
    }

    #endregion 动态资源路径

    public static Dictionary<string, DynamicResoucesInfos> AreaResoucesDic = new Dictionary<string, DynamicResoucesInfos>();

    #region MyRegion

    //// 实景导览
    //SCENERY_GUIDE_FOLDER("/scenery_guide"),
    //// AR土特产
    //SCAN_NATIVE_PRODUCT("/scan_native_product"),
    //// AR门票
    //SCAN_TICKET("/scan_ticket"),
    //// AR召唤
    //SCAN_CONJURE("/scan_conjure"),
    //// AR扫一扫
    //SCAN_MORE("/scan_more"),
    ////移空换景
    //("/more_change"),

    #endregion MyRegion

}

/// <summary>
/// 资源更新类型
/// </summary>
public enum UpdateType
{
    /// <summary>
    /// 不需要更新
    /// </summary>
    none,

    /// <summary>
    /// 本地没有需要全部下载
    /// </summary>
    full,

    /// <summary>
    /// 需要更新
    /// </summary>
    update,
}

/// <summary>
/// 景区内所有的动态资源
/// </summary>
public class DynamicResoucesInfos
{
    public List<DynamicResourcesInfo> ResourcesInfos = new List<DynamicResourcesInfo>()
    {
            new DynamicResourcesInfo()
            {
                GetAll =  PortClass.Instance.GetAllPanorama,
                CheckUpdate = PortClass.Instance.PanoramaCheck,
                LocalPath = PublicAttribute.PanoramaPath,
                DIS = new List<DynamicItem>(),
                ResourcesKey= "vsz-more-change"
            },
            new DynamicResourcesInfo()
            {
                GetAll =  PortClass.Instance.GetAllConjure,
                CheckUpdate = PortClass.Instance.ConjureCheck,
                LocalPath = PublicAttribute.Scan_Conjure,
                DIS = new List<DynamicItem>(),
                ResourcesKey= "vsz-scan-conjure"
            },
            //new DynamicResourcesInfo()
            //{
            //    GetAll =  PortClass.Instance.GetAllNavigation,
            //    CheckUpdate = PortClass.Instance.NavigationCheck,
            //    LocalPath = PublicAttribute.NavigationPath,
            //    DIS = new List<DynamicItem>(),
            //    ResourcesKey = "scenery_guide"
            //},
            new DynamicResourcesInfo()
            {
                GetAll =  PortClass.Instance.GetAllProduct,
                CheckUpdate = PortClass.Instance.ProductCheck,
                LocalPath = PublicAttribute.Scan_ProductPath,
                 DIS = new List<DynamicItem>(),
                ResourcesKey = "vsz-scan-native-product"
            },
            new DynamicResourcesInfo()
            {
                GetAll =  PortClass.Instance.GetAllScanMore,
                CheckUpdate = PortClass.Instance.ScanMoreCheck,
                LocalPath = PublicAttribute.Scan_More,
                DIS = new List<DynamicItem>(),
                ResourcesKey = "vsz-scan-more"
            },
            new DynamicResourcesInfo()
            {
                GetAll =  PortClass.Instance.GetAllTicket,
                CheckUpdate = PortClass.Instance.TicketCheck,
                LocalPath = PublicAttribute.Scan_Ticket,
                DIS = new List<DynamicItem>(),
                ResourcesKey = "vsz-scan-ticket"
            }
    };
}

public class DynamicResourcesInfo
{
    /// <summary>
    /// 获取所有 接口
    /// </summary>
    public string GetAll { get; set; }

    /// <summary>
    /// 检查更新 接口
    /// </summary>
    public string CheckUpdate { get; set; }

    /// <summary>
    /// 本地储存路径
    /// </summary>
    public string LocalPath { get; set; }

    /// <summary>
    /// 动态资源资料
    /// </summary>
    public List<DynamicItem> DIS { get; set; }

    /// <summary>
    /// 本地版本号存放的json
    /// </summary>
    public string JsonLocalPath { get; set; }

    /// <summary>
    /// 服务器中资源的字段
    /// </summary>
    public string ResourcesKey { get; set; }

    /// <summary>
    /// 本资源是否需要更新
    /// </summary>
    public UpdateType UT { get; set; }
}