using cn.sharesdk.unity3d;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPartyLogin : MonoBehaviour
{

    public Button wechatLoginBtn;
    public Button qqLoginBtn;
    public Button weiboLoginBtn;

    /// <summary>
    /// 绑定手机号页面
    /// </summary>
    public GameObject BinDingPhonePage;


   

    private string UserID;
    private string ExpiresTime;
    private string UserName;
    private string thirdToken;
    private string UserIcon;
    private string UserGender;

    private ShareSDK ssdk=null;

    private void Start()
    {

        ssdk = GameObject.FindObjectOfType<ShareSDK>();
        if (ssdk==null)
        {
            ssdk = UnityHelper.GetOrCreateComponent<ShareSDK>(this.gameObject);
        }

        ssdk.authHandler = OnAuthResultHandler;
        ssdk.showUserHandler = OnGetUserInfoResultHandler;


        #region 



        if (ssdk.IsClientValid(PlatformType.WeChat) == false)
        {
            wechatLoginBtn.gameObject.SetActive(false);
       
        }
        if (ssdk.IsClientValid(PlatformType.QQ) == false)
        {
            qqLoginBtn.gameObject.SetActive(false);

        }

        #endregion













        //Listeners
        wechatLoginBtn.onClick.AddListener(() =>
        {
            if (LoginUIController.Instance.CheckNetWork() == false)
            {

                LoginUIController.Instance.ShowPopup("网络错误", "请检查是否连接到网络");
                return;
            }

            if (LoginUIController.Instance.VerifyAgreeToggle() == false)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.AgrrToggle);
                return;
            }
            if (ssdk.IsClientValid(PlatformType.WeChat)==false)
            {
                //   LoginUIController.Instance.ShowPopup("", GlobalParameter.NeedWechatClient);
                LoginUIController.Instance.ShowPopup("登录失败", "请使用其他方式登录");
                return;
            }
            if (ssdk.IsAuthorized(PlatformType.WeChat))
            {
                ssdk.CancelAuthorize(PlatformType.WeChat);

            }
            ssdk.authHandler = OnAuthResultHandler;
            ssdk.showUserHandler = OnGetUserInfoResultHandler;
            //是否设置客户端授权，false：使用      
            ssdk.DisableSSO(false);
            ssdk.GetUserInfo(PlatformType.WeChat);
        });

        qqLoginBtn.onClick.AddListener(() =>
        {
            if (LoginUIController.Instance.CheckNetWork() == false)
            {

                LoginUIController.Instance.ShowPopup("网络错误", "请检查是否连接到网络");
                return;
            }

            if (LoginUIController.Instance.VerifyAgreeToggle() == false)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.AgrrToggle);
                return;
            }
            if (ssdk.IsClientValid(PlatformType.QQ) == false)
            {
                // LoginUIController.Instance.ShowPopup("", GlobalParameter.NeedQQClient);
                LoginUIController.Instance.ShowPopup("登录失败", "请使用其他方式登录");
                return;
            }
            if (ssdk.IsAuthorized(PlatformType.QQ))
            {
                ssdk.CancelAuthorize(PlatformType.QQ);

            }
            ssdk.authHandler = OnAuthResultHandler;
            ssdk.showUserHandler = OnGetUserInfoResultHandler;
            ssdk.DisableSSO(false);
            ssdk.GetUserInfo(PlatformType.QQ);
        });

        weiboLoginBtn.onClick.AddListener(() =>
        {
            if (LoginUIController.Instance.CheckNetWork() == false)
            {

                LoginUIController.Instance.ShowPopup("网络错误", "请检查是否连接到网络");
                return;
            }


            if (LoginUIController.Instance.VerifyAgreeToggle() == false)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.AgrrToggle);
                return;
            }
            //if (ssdk.IsClientValid(PlatformType.SinaWeibo) == false)
            //{
            //   // LoginUIController.Instance.ShowPopup("", GlobalParameter.NeedSinaWeiboClient);

            //    LoginUIController.Instance.ShowPopup("登录失败","请使用其他方式登录");
            //    return;
            //}
            if (ssdk.IsAuthorized(PlatformType.SinaWeibo))
            {
                ssdk.CancelAuthorize(PlatformType.SinaWeibo);

            }
            ssdk.authHandler = OnAuthResultHandler;
            ssdk.showUserHandler = OnGetUserInfoResultHandler;
            //ssdk.DisableSSO(false);
            ssdk.GetUserInfo(PlatformType.SinaWeibo);
        });
    }
    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            //Debug.Log("get user info result :");
            //Debug.Log(JsonMapper.ToJson(result));
            //Debug.Log("AuthInfo:" + JsonMapper.ToJson(ssdk.GetAuthInfo(type)));
            //Debug.Log("Get userInfo success !Platform :" + type);

            //授权成功
            var authinfo = ssdk.GetAuthInfo(type);
            string temp = JsonMapper.ToJson(authinfo);
            Debug.Log("返回信息为   :  "+temp);

            var item = JsonMapper.ToObject(temp);

            UserID = item["userID"].ToString();
        
            UserName = item["userName"].ToString();
        
            UserIcon = item["userIcon"].ToString();
        
            Debug.Log("UserID  " + UserID + "   昵称   " + UserName + "  头像 " + UserIcon + " 性别" + UserGender + " Token" +   thirdToken);

            HttpManager.Instance.ThirdPartyLogin(UserID, (i =>
            {
                switch (i)
                {
                    case 1013:
                      Debug.Log("未绑定手机号");
                        GlobalParameter.ThirduserID = UserID;
                        Debug.Log("GlobalParameter.ThirduserID===" + GlobalParameter.ThirduserID);
                        GlobalParameter.UserName = UserName;
                        Debug.Log("GlobalParameter.UserName===" + GlobalParameter.UserName);
                        GlobalParameter.UserIcon = UserIcon;
                        Debug.Log("GlobalParameter.UserIcon===" + GlobalParameter.UserIcon);


                        LoginUIController.Instance.SetNextUIState(LoginUIState.BinDingPhonePanel);
                        Debug.Log("BinDingPhonePanel");
                        break;
                    case 1010:
                        Debug.Log("登录成功");
                        LoginUIController.Instance.PopupInfo("Third");
                     
                        break;
                    default:
                        Debug.Log("登录异常");
                        LoginUIController.Instance.PopupInfo("Error");
                        break;
                }
            }));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            if (result != null && result.Count > 0)
            {
                //print("authorize success !" + "Platform :" + type + "result:" + MiniJSON.jsonEncode(result));
                Debug.Log("登录成功！ 平台为    "+type  +  "       "+ JsonMapper.ToJson(result));
            }
            else
            {
                print("authorize success !" + "Platform :" + type);
            }
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {

            #region   QQ
            UserID = "DFBA3FA99B5F58ADFCF99CF9309CDABD";

            UserName = "Elvira_Z";

            UserIcon = "http://thirdqq.qlogo.cn/qqapp/1107697201/DFBA3FA99B5F58ADFCF99CF9309CDABD/100";
            //        Token = item["Token"].ToString();
            #endregion


            #region WECHAT
            //UserID = "DFBA3FA99B5F58ADFCF99CF9309CDABD";

            //UserName = "Elvira_Z";

            //UserIcon = "http://thirdqq.qlogo.cn/qqapp/1107697201/DFBA3FA99B5F58ADFCF99CF9309CDABD/100";
            //     Token = item["Token"].ToString();
            #endregion


            #region   SINA
            //UserID = "3173707453";

            //UserName = "暮里灯迟";

            //UserIcon = "http://tvax2.sinaimg.cn/default/images/default_avatar_male_180.gif";
         //   Token = item["Token"].ToString();
            #endregion


            HttpManager.Instance.ThirdPartyLogin(UserID, (i =>
            {
                switch (i)
                {
                    case 1013:
                        Debug.Log("未绑定手机号");
                        GlobalParameter.ThirduserID = UserID;
                        Debug.Log("GlobalParameter.ThirduserID===" + GlobalParameter.ThirduserID);
                        GlobalParameter.UserName = UserName;
                        Debug.Log("GlobalParameter.UserName===" + GlobalParameter.UserName);
                        GlobalParameter.UserIcon = UserIcon;
                        Debug.Log("GlobalParameter.UserIcon===" + GlobalParameter.UserIcon);
                        LoginUIController.Instance.SetNextUIState(LoginUIState.BinDingPhonePanel);
                        Debug.Log("BinDingPhonePanel");
                        break;
                    case 1010:
                        Debug.Log("登录成功");
                        LoginUIController.Instance.PopupInfo("Third");

                        break;
                    default:
                        Debug.Log("登录异常");
                        LoginUIController.Instance.PopupInfo("Error");
                        break;
                }
            }));
        }
    }
}

