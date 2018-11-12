using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalParameter
{

    public static string ak = "YPFhLMn0AZgjAtydA3qhZ4snPc6vQtjn";
    public static string htmlPath = "CustomOverlay.html";

    public static string functionName = "PointOverlap";



    public static int maxCount = 4;
    public static int rowOffset = 320;
    public static string LinkWebURL = "http://47.104.182.82:7070/share/nav/ar?mobid=";
    public static string defaultFont = "songti";

    public static bool isNeedRestore = false;
    public   static int lastPushId = -1;
    /// <summary>
    /// 是否是游客登录
    /// </summary>
    public static bool isVisitor = false;


    public static string nextSceneName;


    //删除委托的原型
    public delegate void OnDestroyHandle(GameObject obj);
    //定义委托
    public OnDestroyHandle OnDestroyGo;


    //禁音委托的原型
    public delegate void OnSilenceHandle(float value);
    //定义委托
    public OnSilenceHandle OnSilenceGo;

    public static string nickName = string.Empty;
    public static string phone = string.Empty;
    #region         ShareTitle

    public static string ShareTitle = "AR游";
    public static string ShareContent = "视觉美景+智能呈现  只留精彩，不留遗憾";


    #endregion

    #region   登录提示字符串
    public static string WrongFormat = "格式不正确";
    public static string AgrrToggle = "请仔细阅读，并同意用户协议后才能使用本软件。";
    public static string InputPhoneNumber = "请输入正确的手机号";
    public static string RegisterSuccessTitle = "注册成功";
    public static string RegisterSuccessMsgs = "您已成功使用该手机号注册，当前自动登录该账号。";
    public static string NeedWechatClient = "微信客户端尚未安装，请选择其他方式登录。";
    public static string NeedQQClient = "QQ客户端尚未安装，请选择其他方式登录。";
    public static string NeedSinaWeiboClient = "新浪微博客户端尚未安装，请选择其他方式登录。";
    public static string InputPassword = "请正确输入密码";
    public static string InputRepeatPassword = "请正确输入密码";
    public static string InputRepeatWrong = "两次输入的密码不一致";
    public static string InputSMSS = "请输入正确的验证码";

    public static string ThirduserID = string.Empty;
    public static string UserName = string.Empty;
    public static string UserIcon = string.Empty;
    internal static string thirdToken = string.Empty;
    internal static string isHasSaw= "本宝宝看过了";




    #endregion
}
