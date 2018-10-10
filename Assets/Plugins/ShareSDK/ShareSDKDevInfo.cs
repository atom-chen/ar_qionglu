using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
    [Serializable]
    public class DevInfoSet
	{
	public	 SinaWeiboDevInfo sinaweibo;
        public QQ qq;
        public WeChat wechat;
        public WeChatMoments wechatMoments;

    }

    public class DevInfo 
	{	
		public bool Enable = true;
	}

    
        //已经修改Android
	[Serializable]
	public class SinaWeiboDevInfo : DevInfo 
	{
#if UNITY_ANDROID
        public const int type = (int) PlatformType.SinaWeibo;
        public string SortId = "4";
        public string AppKey = "1277214325";
        public string AppSecret = "48904ea6a3e667e5aa7253da4880888b";
        public string RedirectUrl = "http://www.visizen.com";
        public bool ShareByAppClient = false;
#elif UNITY_IPHONE
	public	 const int type = (int) PlatformType.SinaWeibo;
	public	 string app_key = "1277214325";
	public	 string app_secret = "48904ea6a3e667e5aa7253da4880888b";
	public	 string redirect_uri = "http://www.visizen.com";
public		 string auth_type = "both";	//can pass "both","sso",or "web"  
#endif
    }


    //已经修改安卓和ios
    [Serializable]
	public class QQ : DevInfo 
	{
		#if UNITY_ANDROID
		public const int type = (int) PlatformType.QQ;
		public string SortId = "3";
		public string AppId = "1107697201";
		public string AppKey = "ixNHtrIa8HT0tjtb";
		public bool ShareByAppClient = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQ;
		public string app_id = "1107697201";
		public string app_key = "ixNHtrIa8HT0tjtb";
		public string auth_type = "both";  //can pass "both","sso",or "web" 
#endif
    }


    //已经修改安卓和ios
    [Serializable]
	public class WeChat : DevInfo 
	{	
		#if UNITY_ANDROID
		public string SortId = "1";
		public const int type = (int) PlatformType.WeChat;
		public string AppId = "wxeb8daa830f75dd72";
		public string AppSecret = "1b184835d62aa12ab80de7e635716828";
		public string UserName = "gh_afb25ac019c9@app";
		public string Path = "/page/API/pages/share/share";
        /// <summary>
        /// false 是不绕过，true是绕过审核
        /// </summary>
		public bool BypassApproval = true;
		public bool WithShareTicket = true;
		public string MiniprogramType = "0";
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wxeb8daa830f75dd72";
		public string app_secret = "1b184835d62aa12ab80de7e635716828";
#endif
    }


    //已经修改安卓和ios
    [Serializable]
	public class WeChatMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "2";
		public const int type = (int) PlatformType.WeChatMoments;
		public string AppId = "wxeb8daa830f75dd72";
		public string AppSecret = "1b184835d62aa12ab80de7e635716828";
		public bool BypassApproval = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "1b184835d62aa12ab80de7e635716828";
#endif
    }





//    //已经修改安卓和ios
//    [Serializable]
//	public class WechatSeries : DevInfo 
//	{
//#if UNITY_ANDROID
//        //for android,please set the configuraion in class "Wechat" ,class "WechatMoments" or class "WechatFavorite"
//        //对于安卓端，请在类Wechat,WechatMoments或WechatFavorite中配置相关信息↑	
//#elif UNITY_IPHONE
//		public const int type = (int) PlatformType.WechatPlatform;
//		public string app_id = "wxeb8daa830f75dd72";
//		public string app_secret = "1b184835d62aa12ab80de7e635716828";
//#endif
//    }
    //已经修改安卓和ios
//    [Serializable]
//	public class QQSeries : DevInfo 
//	{
//#if UNITY_ANDROID
//        //for android,please set the configuraion in class "QQ" and  class "QZone"
//        //对于安卓端，请在类QQ或QZone中配置相关信息↑	
//#elif UNITY_IPHONE
//		public const int type = (int) PlatformType.QQPlatform;
//		public string app_id = "1107697201";
//		public string app_key = "ixNHtrIa8HT0tjtb";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
//#endif
//    }



}
