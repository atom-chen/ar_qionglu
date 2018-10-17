using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoginUISwitch : MonoBehaviour
{

    public Toggle AgreeToggle;


    /// <summary>
    /// 登陆界面弹出提示框
    /// </summary>
    public LoginUIPopupPage PP;


    public GameObject LoginPage;
    public GameObject RegPage;

    /// <summary>
    /// 登录页跳转注册页按钮
    /// </summary>
    public Button LoginPage_RegisterBtn;

    /// <summary>
    /// 注册页返回按钮
    /// </summary>
    public Button RegPage_BackBtn;

    /// <summary>
    /// 帐号密码登陆按钮
    /// </summary>
    public Button UserPwdLoginBtn;

    /// <summary>
    /// 短信验证登陆
    /// </summary>
    public Button SMSSLoginBtn;

    /// <summary>
    /// 登陆页面获取短信验证码按钮
    /// </summary>
    public Button LoginPage_GetSMSSBtn;

    /// <summary>
    /// 注册页面注册按钮
    /// </summary>
    public Button Reg_RegisterBtn;

    /// <summary>
    /// 用户输入帐号
    /// </summary>
    public InputField UserInputField;

    /// <summary>
    /// 用户输入的密码
    /// </summary>
    public InputField PwdInputField;

    /// <summary>
    /// 忘记密码按钮
    /// </summary>
    public Button FogetPwdBtn;

    /// <summary>
    /// 用户协议
    /// </summary>
    public Button AgreementBtn;

    public Button AgressPage_BackBtn;
    public GameObject AgressPageGo;


    /// <summary>
    /// 第三方登录时的OpenID
    /// </summary>
    public string ThirdOpenID;
    /// <summary>
    /// 第三方登录时的用户昵称
    /// </summary>
    public string UserName;
    /// <summary>
    /// 第三方登录时的用户头像
    /// </summary>
    public string UserIcon;



    #region 注册页面

    /// <summary>
    /// 用户输入手机号
    /// </summary>
    public InputField Reg_UserInputField;

    /// <summary>
    /// 用户输入的密码
    /// </summary>
    public InputField Reg_PwdInputField;

    /// <summary>
    /// 用户输入的短信验证码
    /// </summary>
    public InputField Reg_SmsInputField;

    /// <summary>
    /// 获取验证码
    /// </summary>
    public Button Reg_GetSMSBtn;

    #endregion 注册页面

    #region 重置密码页面

    /// <summary>
    /// 返回
    /// </summary>
    public Button ResetPage_BackBtn;

    /// <summary>
    /// 获取验证码
    /// </summary>
    public Button ResetPage_GetSmsBtn;

    /// <summary>
    /// 重置密码按钮
    /// </summary>
    public Button ResetPage_ResetBtn;

    /// <summary>
    /// 用户输入手机号
    /// </summary>
    public InputField ResetPage_UserInputField;

    /// <summary>
    /// 用户输入的密码
    /// </summary>
    public InputField ResetPage_PwdInputField;

    /// <summary>
    /// 用户输入的短信验证码
    /// </summary>
    public InputField ResetPage_SmsInputField;

    /// <summary>
    /// 重置密码页面
    /// </summary>
    public GameObject ResetPwdPage;

    #endregion 重置密码页面

    #region 绑定手机号页面

    public GameObject BindingPage;
    public InputField BindingPage_PhoneNoInputField;
    public InputField BindingPage_SmsCodeInputField;
    public Button BindingPage_BindingBtn;
    public Button BindingPage_GetSMSBtn;
    public Button BindingPage_BackBtn;
    #endregion

    private void Start()
    {
        
    }
    private void Awake()
    {
        //HttpManager.Instance.CheckToken((b =>
        //{
        //   Debug.Log(b);   
        //}));
        AgreementBtn.onClick.AddListener((() =>
        {
            AgressPageGo.SetActive(true);

        }));
        AgressPage_BackBtn.onClick.AddListener((() =>
        {
            AgressPageGo.SetActive(false);
        }));

        LoginPage_RegisterBtn.onClick.AddListener((() =>
        {
            RegPage.SetActive(true);
        }));

        RegPage_BackBtn.onClick.AddListener((() =>
        {
            RegPage.SetActive(false);
        }));
        BindingPage_BackBtn.onClick.AddListener((() =>
        {
            BindingPage.SetActive(false);
        }));
        UserPwdLoginBtn.onClick.AddListener((() =>
        {
            if (!VerifyAgreeToggle())
            {
                return;
            }

            if (VerifyPhoneNo(UserInputField.text)&&VerifyPwd(PwdInputField.text))
            {
                HttpManager.Instance.Login_UserPwd(UserInputField.text, PwdInputField.text, (PopupInfo));
            }
            else
            {
                PP.ShowPopup("格式不正确","请输入正确的格式");
            }
        }));

        SMSSLoginBtn.onClick.AddListener((() =>
        {
            if (!VerifyAgreeToggle())
            {
                return;
            }
            if (VerifyPhoneNo(UserInputField.text)&&VerifySMSCode(PwdInputField.text))
            {
                HttpManager.Instance.Login_SMSS(UserInputField.text, PwdInputField.text, (PopupInfo));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));
        LoginPage_GetSMSSBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(UserInputField.text))
            {
                FreezeButton(LoginPage_GetSMSSBtn);
                HttpManager.Instance.GetSMSS(UserInputField.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));

        Reg_RegisterBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(Reg_UserInputField.text) && VerifyPwd(Reg_PwdInputField.text)&&VerifySMSCode(Reg_SmsInputField.text))
            {
                HttpManager.Instance.Register(Reg_UserInputField.text, Reg_PwdInputField.text, Reg_SmsInputField.text, (PopupInfo));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));

        Reg_GetSMSBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(Reg_UserInputField.text))
            {
                FreezeButton(Reg_GetSMSBtn);
                HttpManager.Instance.GetSMSS(Reg_UserInputField.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));

        ResetPage_GetSmsBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(ResetPage_UserInputField.text))
            {
                FreezeButton(ResetPage_GetSmsBtn);
                HttpManager.Instance.GetSMSS(ResetPage_UserInputField.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));

        ResetPage_ResetBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(ResetPage_UserInputField.text) && VerifyPwd(ResetPage_PwdInputField.text)&& VerifySMSCode(ResetPage_SmsInputField.text))
            {
                HttpManager.Instance.ResetPwd(ResetPage_UserInputField.text, ResetPage_PwdInputField.text,
                ResetPage_SmsInputField.text, (PopupInfo));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));
        ResetPage_BackBtn.onClick.AddListener((() =>
        {
            ResetPwdPage.SetActive(false);
        }));
        FogetPwdBtn.onClick.AddListener((() =>
        {
            ResetPwdPage.SetActive(true);
        }));

        BindingPage_BindingBtn.onClick.AddListener((() =>
        {
            if (VerifyPhoneNo(BindingPage_PhoneNoInputField.text)&&VerifySMSCode(BindingPage_SmsCodeInputField.text))
            {
                HttpManager.Instance.DownloadTexture(UserIcon,"UserIcon.png",PublicAttribute.LocalFilePath+ "APP/",(bytes
                    =>
                {
                    Debug.Log("头像下载成功");
                    HttpManager.Instance.BindingPhoneNo(BindingPage_PhoneNoInputField.text, BindingPage_SmsCodeInputField.text, ThirdOpenID, bytes, UserName, (PopupInfo));
                }));
            }
            else
            {
                PP.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));

        BindingPage_GetSMSBtn.onClick.AddListener((() =>
              {
                  if (VerifyPhoneNo(BindingPage_PhoneNoInputField.text))
                  {
                      FreezeButton(BindingPage_GetSMSBtn);
                      HttpManager.Instance.GetSMSS(BindingPage_PhoneNoInputField.text, (b =>
                      {
                          Debug.Log("获取短信验证码 " + b);
                      }));
                  }
                  else
                  {
                      PP.ShowPopup("格式不正确", "请输入正确的手机号");
                  }
              }));
    }

    /// <summary>
    /// 根据状态码执行
    /// </summary>
    /// <param name="status"></param>
    public  void PopupInfo(string status)
    {
        Debug.Log(status);
        switch (status)
        {
            case "200":
                PP.ShowPopup("请求成功", "请求成功");
                break;
            case "1000":
                PP.ShowPopup("账号或密码错误", "请输入正确的账号或密码");
                break;
            case "1002":
                PP.ShowPopup("手机号已被注册", "请直接使用该手机号登录");
                break;
            case "1004":
                PP.ShowPopup("验证码错误", "请输入正确的验证码");
                break;
            case "1005":
                PP.ShowPopup("手机号格式错误", "请输入正确的手机号码");
                break;
            case "1006":
                PP.ShowPopupAndDoSomeThing("操作成功", "请妥善保管账号和密码",1.5f, InitState);
                //PP.ShowPopup("操作成功", "请妥善保管账号和密码");
                break;
            case "1007":
                PP.ShowPopupAndDoSomeThing("密码修改成功", "请妥善保管账号和密码", 1.5f, InitState);
                break;
            case "1010":
                ScenesManager.Instance.LoadMainScene();
                break;
            case "1011":
                PP.ShowPopup("未设置密码", "密码未设置，请尽快完善密码");
                break;
            case "1012":
                PP.ShowPopupAndDoSomeThing("密码已设置", "请妥善保管账号和密码", 1.5f, InitState);
                break;
            case "1013":
                PP.ShowPopup("未绑定手机号", "请绑定手机号");
                break;
            case "Error":
                PP.ShowPopup("请求失败","请稍候重试");
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 是否同意了用户协议
    /// </summary>
    /// <returns></returns>
   public bool VerifyAgreeToggle()
    {
        if (!AgreeToggle.isOn)
        {
            PP.ShowPopup("用户协议","同意用户协议才能登录");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 验证手机号是否符合格式
    /// </summary>
   bool VerifyPhoneNo(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length!= 11)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 验证密码是否符合格式
    /// </summary>
    bool VerifyPwd(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length<6||str.Length>21)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 验证验证码是否符合格式
    /// </summary>
    bool VerifySMSCode(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length!= 6)
        {
            return false;
        }
        return true;
    }

    private void FreezeButton(Button btn,float time=15)
    {
        Text text = btn.gameObject.GetComponentInChildren<Text>();
        string oldText = text.text;

        StartCoroutine(changeTime(btn, text, time, oldText));
    }

    /// <summary>
    /// 切换到初始化登录界面
    /// </summary>
    void InitState()
    {
        LoginPage.SetActive(true);
        RegPage.SetActive(false);
        ResetPwdPage.SetActive(false);
        BindingPage.SetActive(false);
    }


    IEnumerator changeTime(Button btn,Text text,float time,string oldText)
    {
        btn.interactable = false;
        while (time > 0)
        {
            text.text = time + "";
            //暂停一秒
            yield return new WaitForSeconds(1);
            time--;
        }
        btn.interactable = true;
        text.text = oldText;
    }

}