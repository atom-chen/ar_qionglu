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


    private LoginPanelUI loginPanelUI;

    private string UserID;
    private string ExpiresTime;
    private string UserName;
    private string Token;
    private string UserIcon;
    private string UserGender;

    private ShareSDK ssdk=null;

    private void Awake()
    {
        loginPanelUI = transform.parent.parent.GetComponent<LoginPanelUI>();
        ssdk = GameObject.FindObjectOfType<ShareSDK>();
        if (ssdk==null)
        {
            ssdk = UnityHelper.GetOrCreateComponent<ShareSDK>(this.gameObject);
        }

        ssdk.authHandler = OnAuthResultHandler;
        ssdk.showUserHandler = OnGetUserInfoResultHandler;

        //Listeners
        wechatLoginBtn.onClick.AddListener(() =>
        {
            if (!LoginUIController.Instance.VerifyAgreeToggle())
            {
                return;
            }
            //ssdk.DisableSSO(false);
            ssdk.GetUserInfo(PlatformType.WeChat);
        });

        qqLoginBtn.onClick.AddListener(() =>
        {
            if (!LoginUIController.Instance.VerifyAgreeToggle())
            {
                return;
            }
            //ssdk.DisableSSO(false);
            ssdk.GetUserInfo(PlatformType.QQ);
        });

        weiboLoginBtn.onClick.AddListener(() =>
        {
            if (!LoginUIController.Instance.VerifyAgreeToggle())
            {
                return;
            }
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
            //ExpiresTime = item["expiresTime"].ToString();
            UserName = item["userName"].ToString();
            Token = item["token"].ToString();
            UserIcon = item["userIcon"].ToString();
            //UserGender = item["userGender"].ToString();
            Debug.Log("UserID  " + UserID + "   昵称   " + UserName + "  头像 " + UserIcon + " 性别" + UserGender+"    Token    "+Token);

            HttpManager.Instance.ThirdPartyLogin(Token,(i =>
            {
                switch (i)
                {
                    case 1:
                      Debug.Log("未绑定手机号");
                        LoginUIController.Instance.SetNextUIState(LoginUIState.BinDingPhonePage);
                        loginPanelUI.ThirdOpenID = UserID;
                        loginPanelUI.UserName = UserName;
                        loginPanelUI.UserIcon = UserIcon;
                        break;
                    case 2:
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


}

