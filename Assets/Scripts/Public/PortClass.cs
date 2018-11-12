using UnityEditor;

public class PortClass : Singleton<PortClass>
{
    #region 老版到此一游

    private string category;

    /// <summary>
    /// 获取大类的目录    森林/湖泊/雪地/....
    /// http://192.168.5.48:9090/category/android/all
    /// </summary>
    public string Category
    {
        get
        {
            if (string.IsNullOrEmpty(category))
            {
                category = PublicAttribute.URL + "/category/" + PublicAttribute.Platform + "/all";
            }
            return category;
        }
        set { category = value; }
    }

    private string component;

    /// <summary>
    /// 获取所有的道具信息  海鸥/石头
    /// http://192.168.5.48:9090/component/android/all
    /// </summary>
    public string Component
    {
        get
        {
            component = PublicAttribute.URL + "/component/" + PublicAttribute.Platform + "/all";
            return component;
        }
        set { component = value; }
    }

    private string check;

    /// <summary>
    /// 道具更新  根据本地版本获取更新
    /// http://192.168.5.48:9090/component/version/android/check/update/道具ID/道具版本
    /// </summary>
    public string CheckUpdate
    {
        get
        {
            check = PublicAttribute.URL + "/component/version/" + PublicAttribute.Platform + "/check/update/";
            return check;
        }
        set { check = value; }
    }

    private string fullJson;

    /// <summary>
    /// 获取完整json文件
    /// http://192.168.5.48:9090/resource/online-updating/cp/android/{componentId}/full/{version}
    /// </summary>
    public string FullJsonJson
    {
        get
        {
            if (string.IsNullOrEmpty(fullJson))
            {
                fullJson = PublicAttribute.URL + "/resource/online-updating/cp/" + PublicAttribute.Platform + "/";
            }
            return fullJson;
        }
        set { fullJson = value; }
    }

    private string shareUpload;

    /// <summary>
    /// 道具分享上传  Post  文字/语音  http://192.168.5.48:9090/share/{key｝
    /// </summary>
    public string ShareUpload
    {
        get
        {
            if (string.IsNullOrEmpty(shareUpload))
            {
                shareUpload = PublicAttribute.URL + "/share";
            }
            return shareUpload;
        }
        set { shareUpload = value; }
    }

    private string shareInfoDownload;

    /// <summary>
    /// 道具分享下载 Get  http://192.168.5.48:9090/share/{key}
    /// key   分享的密钥
    /// </summary>
    public string ShareInfoDownload
    {
        get
        {
            if (string.IsNullOrEmpty(shareInfoDownload))
            {
                shareInfoDownload = PublicAttribute.URL + "/share/";
            }
            return shareInfoDownload;
        }
        set { shareInfoDownload = value; }
    }

    private string shareDownload;

    /// <summary>
    /// 分享的资源文件
    /// http://192.168.5.48:9090/resource/{category}/{key}/{filename}
    /// </summary>
    public string ShareDownload
    {
        get
        {
            if (string.IsNullOrEmpty(shareDownload))
            {
                shareDownload = PublicAttribute.URL + "/resource/";
            }
            return shareDownload;
        }
        set { shareDownload = value; }
    }

    private string preview;

    /// <summary>
    /// 获取道具的预览资源
    /// http://192.168.5.48:9090/component/preview/{componentId}
    /// </summary>
    public string Preview
    {
        get
        {
            if (string.IsNullOrEmpty(preview))
            {
                preview = PublicAttribute.URL + "/component/preview/";
            }
            return preview;
        }
        set { preview = value; }
    }

    private string downloadPreview;

    /// <summary>
    /// 下载预览资源的链接
    /// http://192.168.5.48:9090/resource/{category}/{type}/{componentId}/{filename}
    /// </summary>
    public string DownloadPreview
    {
        get
        {
            if (string.IsNullOrEmpty(downloadPreview))
            {
                downloadPreview = PublicAttribute.URL + "/";
            }
            return downloadPreview;
        }
        set { downloadPreview = value; }
    }

    private string guid;

    /// <summary>
    /// 获取引导页 http://192.168.5.48:9090/page/all
    /// </summary>
    public string Guid
    {
        get
        {
            if (string.IsNullOrEmpty(guid))
            {
                guid = PublicAttribute.URL + "/page/all";
            }
            return guid;
        }
        set { guid = value; }
    }

    private string getPagePerview;

    /// <summary>
    /// 获取引导页资源
    /// </summary>
    public string GetPagePerview
    {
        get
        {
            if (string.IsNullOrEmpty(getPagePerview))
            {
                getPagePerview = PublicAttribute.URL + "/page/preview/";
            }
            return getPagePerview;
        }
        set { getPagePerview = value; }
    }

    #endregion 老版到此一游

    #region 获取景点的信息
    private string scenicSpot;

    /// <summary>
    ///  获取景点的信息
    /// http://192.168.30.22:9090/scenery_dot/listByArea?areaId=1
    /// areID --> 景区ID
    /// </summary>
    public string ScenicSpot
    {
        get
        {
            if (string.IsNullOrEmpty(scenicSpot))
            {
                scenicSpot = PublicAttribute.URL + "vsz-scenery-dot/listByArea?areaId=" + PublicAttribute.AreaId;
            }
            return scenicSpot;
        }
        set { scenicSpot = value; }
    }
    #endregion

    #region 主页中特色景点信息
    private string  traitScenicSpot;
    /// <summary>
    /// 特色景点信息
    /// </summary>
    public string TraitScenicSpot
    {
        get
        {
            if (string.IsNullOrEmpty(traitScenicSpot))
            {
                traitScenicSpot = PublicAttribute.URL + "vsz-special-scenery/listByArea?areaId=" + 1;
            }
            return traitScenicSpot;
        }
        set { traitScenicSpot = value; }
    }

    #endregion

    #region 主页中的商家信息

    private string  shopInfo;
    /// <summary>
    /// 主页中的商家信息
    /// </summary>
    public string  ShopsInfo
    {
        get
        {
            if (string.IsNullOrEmpty(shopInfo))
            {
                shopInfo = PublicAttribute.URL + "vsz-restaurant/listByArea?areaId=" + 1;
            }
            return shopInfo;
        }
        set { shopInfo = value; }
    }
    private string  HotelInfo;
    /// <summary>
    /// 主页中的商家信息
    /// </summary>
    public string  HotelsInfo
    {
        get
        {
            if (string.IsNullOrEmpty(HotelInfo))
            {
                HotelInfo = PublicAttribute.URL + "vsz-hotel/listByArea?areaId=" + 1;
            }
            return HotelInfo;
        }
        set { HotelInfo = value; }
    }
    #endregion

    #region 主页中的土特产信息

    private string localSpecialty;
    /// <summary>
    /// 主页中的土特产信息
    /// </summary>
    public string LocalSpecialty
    {
        get
        {
            if (string.IsNullOrEmpty(localSpecialty))
            {
                localSpecialty = PublicAttribute.URL + "vsz-special-product/listByArea?areaId=" + 1;
            }
            return localSpecialty;
        }
        set { localSpecialty = value; }
    }
    #endregion

    #region 获取景区信息列表

    private string areaInfo;
    /// <summary>
    /// 获取景区信息列表
    /// </summary>
    public string AreaInfo
    {
        get
        {
            if (string.IsNullOrEmpty(areaInfo))
            {
                areaInfo = PublicAttribute.URL + "vsz-scenery-area/list";
            }
            return areaInfo;
        }
        set { areaInfo = value; }
    }


    #endregion

    #region 登录

    private string userpwdLogin;

    /// <summary>
    /// 使用帐号密码登陆  POST方法
    /// http://192.168.30.22:9999/login     {"username":"13547808543","password":"282991"}
    /// </summary>
    public string UserPwdLogin
    {
        get
        {
            if (string.IsNullOrEmpty(userpwdLogin))
            {
                userpwdLogin = PublicAttribute.URL + "login";
            }
            return userpwdLogin;
        }
        set { userpwdLogin = value; }
    }

    private string smssLogin;

    /// <summary>
    /// 使用短信验证码登陆 POST
    /// http://192.168.30.22:9999/auth/sms_login?telephone=13547808543&code=917946
    /// </summary>
    public string SMSLogin
    {
        get
        {
            if (string.IsNullOrEmpty(smssLogin))
            {
                smssLogin = PublicAttribute.URL + "auth/sms_login";
            }
            return smssLogin;
        }
        set { smssLogin = value; }
    }

    private string visitorLogin;
    /// <summary>
    /// 游客登陆 POST
    /// http://192.168.30.22:9999/auth/suggest
    /// </summary>
    public string VisitorLogin
    {
        get
        {
            if (string.IsNullOrEmpty(visitorLogin))
            {
                visitorLogin = PublicAttribute.URL + "auth/suggest";
            }
            return visitorLogin;
        }
        set { visitorLogin = value; }
    }
    private string ChangePho;
    /// <summary>
    /// 使用短信验证码登陆 POST
    /// http://192.168.30.22:9999/user/updateTelephone
    /// </summary>
    public string PhoChange
    {
        get
        {
            if (string.IsNullOrEmpty(ChangePho))
            {
                ChangePho = PublicAttribute.URL + "user/updateTelephone";
            }
            return ChangePho;
        }
        set { ChangePho = value; }
    }

    private string getSMSS;
    /// <summary>
    /// 获取短信验证码 GET
    /// http://192.168.30.22:9999/auth/getCode?telephone=13547808543
    /// </summary>
    public string GetSMSS
    {
        get
        {
            if (string.IsNullOrEmpty(getSMSS))
            {
                getSMSS = PublicAttribute.URL + "auth/getCode?telephone=";
            }
            return getSMSS;
        }
        set { getSMSS = value; }
    }

    private string register;
    /// <summary>
    /// 用户注册接口 POST
    /// {
    /// "username":"15928517460"
    /// "password":"123456",
    /// "code":"695689"
    /// }
    /// </summary>
    public string  Register
    {
        get
        {
            if (string.IsNullOrEmpty(register))
            {
                register = PublicAttribute.URL + "auth/enroll";
            }
            return register;
        }
        set { register = value; }
    }
    #endregion

    #region 重置密码  Post

    private string resetPwd;
    /// <summary>
    /// 重置密码  Post
    /// http://192.168.30.36:8081/arql/auth/upd
    /// {
    ///"username":"15928517460"
    ///"password":"123456",
    ///"code":"695689"
    ///}
    /// </summary>
    public string ResetPwd
    {
        get
        {
            if (string.IsNullOrEmpty(resetPwd))
            {
                resetPwd = PublicAttribute.URL + "auth/upd";
            }
            return resetPwd;
        }
        set { resetPwd = value; }
    }
    #endregion

    #region 第三方登录接口  POST

    private string  thirdPartyLogin;
    /// <summary>
    /// 第三方登录接口  POST
    /// http://192.168.30.22:9999/auth/thirdLogin
    /// </summary>
    public string ThirdPartyLogin
    {
        get
        {
            if (string.IsNullOrEmpty(thirdPartyLogin))
            {
                thirdPartyLogin = PublicAttribute.URL + "auth/thirdLogin";
            }
            return thirdPartyLogin;
        }
        set { thirdPartyLogin = value; }
    }
    #endregion

    #region 第三方注册 绑定手机号

    private string binDingPhoneNo;
    /// <summary>
    /// 绑定手机号   POST
	//"username":13547808543,
	//"code":960426,
	//"thirdCode":"759630194"
    /// </summary>
    public string BinDingPhoneNo
    {
        get
        {
            if (string.IsNullOrEmpty(binDingPhoneNo))
            {
                binDingPhoneNo = PublicAttribute.URL + "auth/thirdEnroll";
            }
            return binDingPhoneNo;
        }
        set { binDingPhoneNo = value; }
    }
    #endregion

    #region 根据Token获取用户信息

    private string  userInfoByToken;
    /// <summary>
    /// 验证Token 是否过期   Get
    /// </summary>
    public string UserInfoByToken
    {
        get
        {
            if (string.IsNullOrEmpty(userInfoByToken))
            {
                userInfoByToken = PublicAttribute.URL + "user/infoByToken";
            }
            return userInfoByToken;
        }
        set { userInfoByToken = value; }
    }
    #endregion

    #region 修改用户昵称
    private string modifiNickName;
    /// <summary>
    /// 修改用户昵称
    /// </summary>
    public string ModifiNickName
    {
        get
        {
            if (string.IsNullOrEmpty(modifiNickName))
            {
                modifiNickName = PublicAttribute.URL + "user/nickName";
            }
            return modifiNickName;
        }
        set { modifiNickName = value; }
    }

    #endregion

    #region 修改用户头像

    private string modifiUserIcon;
    /// <summary>
    /// 修改用户头像
    /// </summary>
    public string ModifiUserIcon
    {
        get
        {
            if (string.IsNullOrEmpty(modifiUserIcon))
            {
                modifiUserIcon = PublicAttribute.URL + "user/userHead";
            }
            return modifiUserIcon;
        }
        set { modifiUserIcon = value; }
    }


    #endregion

    #region 反馈

    private string fankui;
    /// <summary>
    /// 反馈
    /// </summary>
    public string suggest
    {
        get
        {
            if (string.IsNullOrEmpty(fankui))
            {
                fankui = PublicAttribute.URL + "suggest/save";
            }
            return fankui;
        }
        set { fankui = value; }
    }
    #endregion
    
    #region 版本号

    private string appid;
    /// <summary>
    /// 反馈
    /// </summary>
    public string updateid
    {
        get
        {
            if (string.IsNullOrEmpty(appid))
            {
                appid = PublicAttribute.URL + "app/info";
            }
            return appid;
        }
        set { appid = value; }
    }

    #endregion

    #region 退出登录 Post

    private string  logout;
    /// <summary>
    /// 退出登录
    /// </summary>
    public string  Logout
    {
        get
        {
            if (string.IsNullOrEmpty(logout))
            {
                logout = PublicAttribute.URL + "auth/logout";
            }
            return logout;
        }
        set { logout = value; }
    }


    #endregion

    #region 获取移空换景接口

    private string getAllPanorama;
    /// <summary>
    /// 获取所有的移空换景信息
    /// </summary>
    public string GetAllPanorama
    {
        get
        {
            if (string.IsNullOrEmpty(getAllPanorama))
            {
                getAllPanorama = PublicAttribute.URL + "vsz-more-change/all?platform=" + PublicAttribute.PlatformInt;
            }
            return getAllPanorama;
        }
        set { getAllPanorama = value; }
    }



    private string panoramaCheck;
    /// <summary>
    /// 移空换景 检查版本
    /// localhost:9090/more_change/check?version=v0.0.1&typeId=6&staticId=3&platform=1
    /// </summary>
    public string PanoramaCheck
    {
        get
        {
            if (string.IsNullOrEmpty(panoramaCheck))
            {
                panoramaCheck = PublicAttribute.URL + "vsz-more-change/check?version=";
            }
            return panoramaCheck;
        }
        set { panoramaCheck = value; }
    }

    #endregion

    #region 获取实景导览接口

    private string getAllNavigation;
    /// <summary>
    /// 获取所有的实景导览信息
    /// </summary>
    public string GetAllNavigation
    {
        get
        {
            if (string.IsNullOrEmpty(getAllNavigation))
            {
                getAllNavigation = PublicAttribute.URL + "vsz-scenery-guide/all_v2?platform=" + PublicAttribute.PlatformInt+"&areaId="+PublicAttribute.AreaId;
            }
            return getAllNavigation;
        }
        set { getAllNavigation = value; }
    }

    #endregion

    #region 获取AR土特产接口

    private string getAllProduct;
    /// <summary>
    /// 获取所有的AR土特产信息
    /// </summary>
    public string GetAllProduct
    {
        get
        {
            if (string.IsNullOrEmpty(getAllProduct))
            {
                getAllProduct = PublicAttribute.URL + "vsz-scan-native-product/all?platform=" + PublicAttribute.PlatformInt;
            }
            return getAllProduct;
        }
        set { getAllProduct = value; }
    }

    private string productCheck;
    /// <summary>
    /// AR土特产检查版本
    /// localhost:9090/more_change/check?version=v0.0.1&typeId=6&staticId=3&platform=1
    /// </summary>
    public string ProductCheck
    {
        get
        {
            if (string.IsNullOrEmpty(productCheck))
            {
                productCheck = PublicAttribute.URL + "vsz-scan-native-product/check?version=";
            }
            return productCheck;
        }
        set { productCheck = value; }
    }


    #endregion

    #region 获取AR门票接口

    private string getAllTicket;
    /// <summary>
    /// 获取所有的AR门票信息
    /// </summary>
    public string GetAllTicket
    {
        get
        {
            if (string.IsNullOrEmpty(getAllTicket))
            {
                getAllTicket = PublicAttribute.URL + "vsz-scan-ticket/all?platform=" + PublicAttribute.PlatformInt;
            }
            return getAllTicket;
        }
        set { getAllTicket = value; }
    }

    private string ticketCheck;
    /// <summary>
    /// AR门票检查版本
    /// localhost:9090/more_change/check?version=v0.0.1&typeId=6&staticId=3&platform=1
    /// </summary>
    public string TicketCheck
    {
        get
        {
            if (string.IsNullOrEmpty(ticketCheck))
            {
                ticketCheck = PublicAttribute.URL + "vsz-scan-ticket/check?version=";
            }
            return ticketCheck;
        }
        set { ticketCheck = value; }
    }

    #endregion

    #region 获取AR召唤接口

    private string getAllConjure;
    /// <summary>
    /// 获取所有的AR召唤信息
    /// </summary>
    public string GetAllConjure
    {
        get
        {
            if (string.IsNullOrEmpty(getAllConjure))
            {
                getAllConjure = PublicAttribute.URL + "vsz-scan-conjure/all?platform=" + PublicAttribute.PlatformInt;
            }
            return getAllConjure;
        }
        set { getAllConjure = value; }
    }

    private string conjureCheck;
    /// <summary>
    /// AR召唤检查版本
    /// localhost:9090/more_change/check?version=v0.0.1&typeId=6&staticId=3&platform=1
    /// </summary>
    public string ConjureCheck
    {
        get
        {
            if (string.IsNullOrEmpty(conjureCheck))
            {
                conjureCheck = PublicAttribute.URL + "vsz-scan-conjure/check?version=";
            }
            return conjureCheck;
        }
        set { conjureCheck = value; }
    }


    #endregion

    #region 获取AR扫一扫接口

    private string getAllScan;
    /// <summary>
    /// 获取所有的AR扫一扫信息
    /// </summary>
    /// </summary>
    /// </summary>
    public string GetAllScanMore
    {
        get
        {
            if (string.IsNullOrEmpty(getAllScan))
            {
                getAllScan = PublicAttribute.URL + "vsz-scan-more/all?platform=" + PublicAttribute.PlatformInt;
            }
            return getAllScan;
        }
        set { getAllScan = value; }
    }

    private string scanCheck;
    /// <summary>
    /// AR扫一扫检查版本
    /// localhost:9090/more_change/check?version=v0.0.1&typeId=6&staticId=3&platform=1
    /// </summary>
    public string ScanMoreCheck
    {
        get
        {
            if (string.IsNullOrEmpty(scanCheck))
            {
                scanCheck = PublicAttribute.URL + "vsz-scan-more/check?version=";
            }
            return scanCheck;
        }
        set { scanCheck = value; }
    }

    #endregion

    #region 到此一游接口

    #region 获取道具分类

    public string Tohere_TypeList = PublicAttribute.URL + "vsz-to-here/type_list";

    #endregion

    #region 获取所有的道具
    /// <summary>
    /// 获取所有道具
    /// </summary>
    public string Tohere_GetAll = PublicAttribute.URL + "vsz-to-here/all_v2?";

    #endregion

    #region 道具更新
    /// <summary>
    /// localhost:9090/to_here/check_v2?version=v0.0.1&name=石头&&platform=1&areaId=1
    /// </summary>
    public string Tohere_Check = PublicAttribute.URL + "vsz-to-here/check_v2?";

    #endregion

    #endregion

    #region 获取广告内容  （1：启动页，2：广告页，3：引导页，4：Banner）
    /// <summary>
    /// 1：启动页，2：广告页，3：引导页，4：Banner
    /// </summary>
    public string Advertisement = PublicAttribute.URL + "vsz-advertisement/listByType?type=";
    public string PushContent = PublicAttribute.URL + "vsz-scenery-area/list";
    #endregion

}