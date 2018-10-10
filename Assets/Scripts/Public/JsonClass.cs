using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class JsonClass : Singleton<JsonClass>
{

    /// <summary>
    /// 景区信息
    /// </summary>
    public List<AreaInfo>  AreaInfoS;
    /// <summary>
    /// 实景导览信息
    /// </summary>
    public List<NavigationInfo> NavigationInfos;

    /// <summary>
    /// 启动页面
    /// </summary>
    public List<Ads> LaunchPages;
    /// <summary>
    /// 广告页面
    /// </summary>
    public List<Ads> AdsPages;
    /// <summary>
    /// 引导页面
    /// </summary>
    public List<Ads> GuidPages;
    /// <summary>
    /// 主页Banner
    /// </summary>
    public List<Ads> BannerPages; 


    /// <summary>
    /// 到此一游信息
    /// </summary>
    public List<VisitInfo> VisitInfoS;

    /// <summary>
    /// 到此一游分类
    /// </summary>
    public Dictionary<string, string> VisitTypeList; 

    /// <summary>
    /// 所有景点信息列表
    /// </summary>
    public List<ScenicSpotItem> ScenicSpotInfoS { get; set; }


    #region 动态资源信息
    /// <summary>
    /// 所有的全景信息列表
    /// </summary>
    public List<DynamicItem> PanoramaInfoS { get; set; }

    /// <summary>
    /// 所有实景导览信息列表,不用！！！
    /// </summary>
    public List<DynamicItem> NavigationInfoS { get; set; }

    #endregion

    /// <summary>
    /// 主页显示特色景点信息列表
    /// </summary>
    public List<ScenicSpotItem> TraitScenicSpotInfoS { get; set; }
    /// <summary>
    /// 主页展示的商家信息列表
    /// </summary>
    public List<ScenicSpotItem> ShopInfoS { get; set; } 
    /// <summary>
    /// 主页显示的特产信息列表
    /// </summary>
    public List<ScenicSpotItem> LocalSpecialtyS { get; set; }
    protected override void Init()
    {
        base.Init();
        NavigationInfos = new List<NavigationInfo>();
        VisitInfoS = new List<VisitInfo>();
        ScenicSpotInfoS = new List<ScenicSpotItem>();
        PanoramaInfoS = new List<DynamicItem>();
        NavigationInfoS = new List<DynamicItem>();
        TraitScenicSpotInfoS= new List<ScenicSpotItem>();
        ShopInfoS = new List<ScenicSpotItem>();
        LocalSpecialtyS = new List<ScenicSpotItem>();
        AreaInfoS = new List<AreaInfo>();


        LaunchPages= new List<Ads>();
        AdsPages = new List<Ads>();
        GuidPages = new List<Ads>();
        BannerPages = new List<Ads>();


        ServerAppVersionJson = new AppVersionJson();
        LocalAppVersionJson = new AppVersionJson();
        CurrentAppUpdateFileJson = new ResourceFileJson();
        CurrentAppFullFileJson = new ResourceFileJson();
        FilePathJson = new FilePath();
        ScenicsUpdateInfos = new List<ScenicsUpdateInfo>();

        ComponentList = new List<CategoryComponentItem>();
        CategoryList = new List<CategoryComponentItem>();

        GUIDList = new List<GUID>();

        VisitTypeList= new Dictionary<string, string>();

        #region 景点列表

        curAreaTreeJson = new AreaTree();

        #endregion 景点列表
    }



    #region 老版

    /// <summary>
    /// 服务器APP版本号
    /// </summary>
    public AppVersionJson ServerAppVersionJson;

    /// <summary>
    /// 资源文件列表
    /// </summary>
    public ResourceFileJson CurrentAppUpdateFileJson;

    /// <summary>
    /// 当前版本所有文件的列表
    /// </summary>
    public ResourceFileJson CurrentAppFullFileJson;

    /// <summary>
    /// 本地APP版本号
    /// </summary>
    public AppVersionJson LocalAppVersionJson;

    /// <summary>
    /// 所有文件夹路径的Json文件
    /// </summary>
    public FilePath FilePathJson;

    /// <summary>
    /// 景区更新信息
    /// </summary>
    public List<ScenicsUpdateInfo> ScenicsUpdateInfos { get; set; }

    /// <summary>
    /// 所有的大类集合 湖泊/森林
    /// </summary>
    public List<CategoryComponentItem> CategoryList { get; set; }

    /// <summary>
    /// 所有的道具集合 海鸥/石头/....
    /// </summary>
    public List<CategoryComponentItem> ComponentList { get; set; }

    /// <summary>
    /// 所有引导页资源
    /// </summary>
    public List<GUID> GUIDList { get; set; }

    #region 景点列表

    /// <summary>
    /// 景点列表
    /// </summary>
    public AreaTree curAreaTreeJson;

    #endregion 景点列表


    #endregion
}


#region 老版内容

/// <summary>
/// 所有文件夹路径 通过http://192.168.5.246:8080/area/filePath接口获取
/// </summary>
public class FilePath
{
    public List<string> filePaths;

    public FilePath()
    {
        filePaths = new List<string>();
    }
}

public class AppVersionJson
{
    /// <summary>
    /// 完整版本号
    /// </summary>
    public string version { get; set; }
}

public class ResourceFileJson
{
    /// <summary>
    /// 版本
    /// </summary>
    public string version { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// 当前需要执行的操作 update(更新)/full（全部删除再更新）
    /// </summary>
    public string op { get; set; }

    /// <summary>
    /// 文件列表
    /// </summary>
    public List<FilesItem> files { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int primaryNo { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int subNo { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int updateNo { get; set; }

    /// <summary>
    /// 所有文件的大小
    /// </summary>
    public string size { get; set; }

    public ResourceFileJson()
    {
        files = new List<FilesItem>();
    }
}

/// <summary>
/// 道具类文件列表
/// </summary>
public class ComponentFileItem
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 文件格式
    /// </summary>
    public string suffix { get; set; }

    /// <summary>
    /// 文件保存路径
    /// </summary>
    public string path { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public string size { get; set; }

    /// <summary>
    /// md5值
    /// </summary>
    public string md5 { get; set; }
}

public class FilesItem
{
    public FilesItem()
    {
        paths = new List<string>();
    }

    /// <summary>
    /// 文件名
    /// </summary>
    public string originalName { get; set; }

    /// <summary>
    /// 下载链接
    /// </summary>
    public string url { get; set; }

    /// <summary>
    /// 文件存放路径
    /// </summary>
    public List<string> paths { get; set; }

    /// <summary>
    /// 操作方式
    /// </summary>
    public string op { get; set; }

    /// <summary>
    /// md5值
    /// </summary>
    public string md5 { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public string size { get; set; }
}

#region 景点列表

/// <summary>
/// 景点
/// </summary>
public class ScenicsPointItem
{
    /// <summary>
    ///
    /// </summary>
    public int areaId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string> children { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaContent { get; set; }

    /// <summary>
    /// 光福寺
    /// </summary>
    public string areaName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string imgPath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaLogo { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string filePath { get; set; }

    public ScenicsPointItem()
    {
        children = new List<string>();
    }
}

/// <summary>
/// 景区
/// </summary>
public class ScenicsSpotItem
{
    /// <summary>
    ///
    /// </summary>
    public int areaId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<ScenicsPointItem> ScenicsPointList { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaContent { get; set; }

    /// <summary>
    /// 光福寺
    /// </summary>
    public string areaName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string imgPath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaLogo { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string filePath { get; set; }

    public ScenicsSpotItem()
    {
        ScenicsPointList = new List<ScenicsPointItem>();
    }
}

/// <summary>
/// 县
/// </summary>
public class CountyItem
{
    /// <summary>
    ///
    /// </summary>
    public int areaId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<ScenicsSpotItem> ScenicList { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaContent { get; set; }

    /// <summary>
    /// 西昌市
    /// </summary>
    public string areaName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string imgPath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaLogo { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string filePath { get; set; }

    public CountyItem()
    {
        ScenicList = new List<ScenicsSpotItem>();
    }
}

/// <summary>
/// 省
/// </summary>
public class ProvinceItem
{
    /// <summary>
    ///
    /// </summary>
    public int areaId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<CountyItem> CountyList { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaContent { get; set; }

    /// <summary>
    /// 四川
    /// </summary>
    public string areaName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string imgPath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string areaLogo { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string filePath { get; set; }

    public ProvinceItem()
    {
        CountyList = new List<CountyItem>();
    }
}

public class AreaTree
{
    /// <summary>
    ///
    /// </summary>
    public List<ProvinceItem> ProvinceList { get; set; }

    public AreaTree()
    {
        ProvinceList = new List<ProvinceItem>();
    }
}

#endregion 景点列表

public class ScenicsUpdateInfo
{
    /// <summary>
    /// 景区ID
    /// </summary>
    public string scenicSpotId { get; set; }

    /// <summary>
    /// 县市ID
    /// </summary>
    public string parentId { get; set; }

    /// <summary>
    /// 景区名字
    /// </summary>
    public string areaName { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string version { get; set; }

    /// <summary>
    /// 大小
    /// </summary>
    public string size { get; set; }

    /// <summary>
    /// 更新方式
    /// </summary>
    public string op { get; set; }

    /// <summary>
    /// 文件列表
    /// </summary>
    public List<FilesItem> files { get; set; }

    public ScenicsUpdateInfo()
    {
        files = new List<FilesItem>();
    }
}

/// <summary>
/// 大类/道具的ITem
/// </summary>
public class CategoryComponentItem
{
    public CategoryComponentItem()
    {
        pids = new List<int>();
        paths = new List<string>();
        previews = new List<ComponentFileItem>();
    }

    /// <summary>
    /// 父文件id
    /// </summary>
    public List<int> pids { get; set; }

    /// <summary>
    /// 自身文件id
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// 英文 名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public List<string> paths { get; set; }

    /// <summary>
    /// 中文名称
    /// </summary>
    public string des { get; set; }

    /// <summary>
    /// 缩略图/视频/GIF
    /// </summary>
    public List<ComponentFileItem> previews { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string version { get; set; }
}

/// <summary>
/// 文字结构
/// </summary>
public class TextType
{
    /// <summary>
    /// 内容
    /// </summary>
    public string content { get; set; }

    /// <summary>
    /// 字体
    /// </summary>
    public string FontType { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    public string color { get; set; }
}


public class GUID
{
    public GUID()
    {
        CFIS = new List<ComponentFileItem>();
    }
    /// <summary>
    /// id
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; } 
    /// <summary>
    /// index排序
    /// </summary>
    public int pageIndex { get; set; }
    /// <summary>
    /// 对应的网站
    /// </summary>
    public string url { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string des { get; set; }
    /// <summary>
    /// 具体属性
    /// </summary>
    public List<ComponentFileItem>  CFIS { get; set; }

}
#endregion


#region 景点信息Json
/// <summary>
/// 缩略图内容
/// </summary>
public class Thumbnail
{
    /// <summary>
    /// 缩略图ID
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 缩略图的名称
    /// </summary>
    public string md5 { get; set; }
    /// <summary>
    /// 缩略图后缀名
    /// </summary>
    public string extName { get; set; }
    /// <summary>
    /// 缩略图大小
    /// </summary>
    public string size { get; set; }
    /// <summary>
    /// 缩略图下载地址
    /// </summary>
    public string address { get; set; }

    /// <summary>
    /// 本地路径
    /// </summary>
    public string localPath { get; set; }

    /// <summary>
    /// 用于OSS下载的 关键字
    /// </summary>
    public string endPoint { get; set; }
}
/// <summary>
/// 景区信息
/// </summary>
public class SceneryArea
{
    /// <summary>
    /// 景区ID
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 景区名称
    /// </summary>
    public string name { get; set; }
}
/// <summary>
/// 景点信息
/// </summary>
public class ScenicSpotItem
{
    /// <summary>
    /// 景点ID
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 景点名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 景点缩略图信息
    /// </summary>
    public Thumbnail thumbnail { get; set; }
    /// <summary>
    /// 景点经度
    /// </summary>
    public string locationX { get; set; }
    /// <summary>
    /// 景点纬度
    /// </summary>
    public string locationY { get; set; }
    /// <summary>
    /// 景点海拔
    /// </summary>
    public string height { get; set; }
    /// <summary>
    /// 所属的景区
    /// </summary>
    public SceneryArea sceneryArea { get; set; }
    /// <summary>
    /// 网页详情地址
    /// </summary>
    public string address { get; set; }
    /// <summary>
    /// 景点资源描述
    /// </summary>
    public string description { get; set; }

}

#endregion


#region 动态资源信息Json
/// <summary>
/// 动态资源信息
/// </summary>
public class DynamicItem
{
    /// <summary>
    /// 全景点ID
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 景点的类型
    /// </summary>
    public string typeId { get; set; }

    /// <summary>
    /// 景点分类 
    /// </summary>
    public string typeName { get; set; }

    /// <summary>
    /// 全景点的版本号
    /// </summary>
    public string versionName { get; set; }
    /// <summary>
    /// 静态资源
    /// </summary>
    public BaseEntity baseEntity { get; set; }
    /// <summary>
    /// 所有的资源
    /// </summary>
    public List<VersionFilesItem> VersionFilesItems { get; set; }
    /// <summary>
    /// 资源类型
    /// </summary>
    public string type { get; set; }
    public DynamicItem()
    {
        VersionFilesItems = new List<VersionFilesItem>();
    }
    /// <summary>
    /// 移空换景中的卡券图
    /// </summary>
    public Thumbnail PageThumbnail { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string description { get; set; }
}
/// <summary>
/// 静态资源
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// 静态资源ID
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 静态资源名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 静态资源缩略图
    /// </summary>
    public Thumbnail thumbnail { get; set; }
    /// <summary>
    /// 静态资源对应的景区
    /// </summary>
    public SceneryArea sceneryArea { get; set; }
    /// <summary>
    /// 全景点经度
    /// </summary>
    public string locationX { get; set; }
    /// <summary>
    /// 全景点纬度
    /// </summary>
    public string locationY { get; set; }
    /// <summary>
    /// 全景点海拔
    /// </summary>
    public string height { get; set; }
    /// <summary>
    /// 网页详情地址
    /// </summary>
    public string address { get; set; }
    /// <summary>
    /// 全景点详情
    /// </summary>
    public string content { get; set; }
    /// <summary>
    /// 景点资源描述
    /// </summary>
    public string description { get; set; }

}
/// <summary>
/// 版本文件
/// </summary>
public class VersionFilesItem
{
    /// <summary>
    /// md5
    /// </summary>
    public string md5 { get; set; }
    /// <summary>
    /// 文件名称
    /// </summary>
    public string filename { get; set; }
    /// <summary>
    /// 后缀名
    /// </summary>
    public string extName { get; set; }
    /// <summary>
    /// 大小
    /// </summary>
    public string size { get; set; }
    /// <summary>
    /// 下载地址
    /// </summary>
    public string address { get; set; }
    /// <summary>
    /// 本地存储路径
    /// </summary>
    public  string localPath { get; set; }
    /// <summary>
    /// OSS下载文件关键字
    /// </summary>
    public string endPoint { get; set; }
}

/// <summary>
/// 全景的本地信息
/// </summary>
public class LocalPannoramaInfo
{
   public List<DynamicItem> PIS;

    public LocalPannoramaInfo()
    {
        PIS = new List<DynamicItem>();
    }
}
/// <summary>
/// 服务器的全景信息
/// </summary>
public class ServerPanoramaInfo
{
    public List<DynamicItem> PIS;

    public ServerPanoramaInfo()
    {
        PIS = new List<DynamicItem>();
    }
}

#endregion


#region 用户信息

public class UserInfo
{
    /// <summary>
    /// 用户手机号
    /// </summary>
    public string PhoneNo;
    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName;
    /// <summary>
    /// 用户头像
    /// </summary>
    public Sprite UserIcon;
}

#endregion


#region 景区信息

/// <summary>
/// 景区信息
/// </summary>
public class AreaInfo
{
    /// <summary>
    /// 景区id
    /// </summary>
    public string id;

    /// <summary>   
    ///  景区描述
    /// </summary>
    public string describe;

    /// <summary>
    /// 景区中文名称
    /// </summary>
    public string name;
}


#endregion


#region 系统通知
/// ARScan
//	AR扫一扫/AR门票/AR召唤
//  Panorama
//	移空换景
//  Navigation
//	实景导览
//  Visit
//	到此一游
//  None
//	无操作


/// <summary>
/// 系统通知的内容
/// </summary>
public class NotifyType
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public string action { get; set; }
    /// <summary>
    /// 对应的id
    /// </summary>
    public string id { get; set; }
}

#endregion


#region 到此一游
/// <summary>
/// 到此一游信息
/// </summary>
public class VisitInfo
{
    /// <summary>
    /// id
    /// </summary>
    public string id;
    /// <summary>
    /// 版本号
    /// </summary>
    public string versionName;
    /// <summary>
    /// 道具的名称
    /// </summary>
    public string name;
    /// <summary>
    /// 道具的分类
    /// </summary>
    public string typeIds;
    /// <summary>
    /// 道具的描述
    /// </summary>
    public string description;
    /// <summary>
    /// 所有的资源
    /// </summary>
    public List<VersionFilesItem> VersionFilesItems { get; set; }
    /// <summary>
    /// 缩略图信息
    /// </summary>
    public Thumbnail Thumbnail;
    /// <summary>
    /// 卡卷式缩略图
    /// </summary>
    public Thumbnail PageThumbnail;

    /// <summary>
    /// 资源更新类型
    /// </summary>
    public UpdateType UT;
    public VisitInfo()
    {
        VersionFilesItems= new List<VersionFilesItem>();
    }
}

#endregion


#region 实景导览

public class NavigationInfo
{
    /// <summary>
    /// 实景导览的ID
    /// </summary>
    public string id { get; set; }
    /// <summary>
    /// 静态资源
    /// </summary>
    public BaseEntity baseEntity { get; set; }
    /// <summary>
    /// 分类
    /// </summary>
    public string type { get; set; }
}

#endregion

#region 广告接口 1：启动页，2：广告页，3：引导页，4：Banner

public class Ads
{
    public string id { get; set; }
    /// <summary>
    /// 排序   排序越大越靠前
    /// </summary>
    public string order { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    public string content { get; set; }
    /// <summary>
    /// 类型 1：启动页，2：广告页，3：引导页，4：Banner
    /// </summary>
    public string type { get; set; }

    /// <summary>
    /// 对应的网页地址
    /// </summary>
    public string address { get; set; }

    /// <summary>
    /// 缩略图
    /// </summary>
    public Thumbnail Thumbnail { get; set; }
}

#endregion
