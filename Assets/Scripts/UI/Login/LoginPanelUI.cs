using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoginPanelUI : UIWindowsBase
{

    #region   Accounts
    [Header("账户登录区域")]
    public GameObject accountsGo;
    /// <summary>
    /// 登录页手机快速注册按钮
    /// </summary>
    public Button registerBtn;


    public Button visitorBtn;
    public Button ForgetPwdBtn;


    public InputField AccountsphoneInput;
    public InputField AccountsPwInpPut;
    public Button AccountsUserPwdLogin;
    public Button AccountseyeBtn;


    public Toggle UserType;
    #endregion


    #region  SMSS

    [Header("手机验证码登录区域")]
    public GameObject smssGo;
    public InputField SMSSphoneInput;
    public InputField SMSSPwInpPut;
    public Button SMSSgetSMSSBtn;
    public Button SMSSUserSMSSLogin;


    public Toggle SMSSType;
    #endregion
    #region   三方登录



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
    #endregion
    public override void Awake()
    {
        base.Awake();
        ForgetPwdBtn.onClick.AddListener(() => {

            LoginUIController.Instance.SetNextUIState(LoginUIState.ForgetPwdPanel);

        });

        #region   账号密码响应事件
        //手机号注册按钮
        registerBtn.onClick.AddListener((() =>
            {
                LoginUIController.Instance.SetNextUIState(LoginUIState.RegistPanel);
            }));
        //账号密码登录按钮
        AccountsUserPwdLogin.onClick.AddListener((() =>
        {
            if (!LoginUIController.Instance.VerifyAgreeToggle())
            {
                return;
            }
            if (string.IsNullOrEmpty(AccountsphoneInput.text))
            {
                LoginUIController.Instance.ShowPopup("", "请输入登录账号");
            }
            else if (string.IsNullOrEmpty(AccountsPwInpPut.text))
            {
                LoginUIController.Instance.ShowPopup("", "请输入密码");
            }
            else if (LoginUIController.Instance.VerifyPhoneNo(AccountsphoneInput.text) && LoginUIController.Instance.VerifyPwd(AccountsPwInpPut.text))
            {
                HttpManager.Instance.Login_UserPwd(AccountsphoneInput.text, AccountsPwInpPut.text, (LoginUIController.Instance.PopupInfo));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("登录失败", "请输入正确的账号和密码");
            }
        }));
        AccountseyeBtn.onClick.AddListener((() =>
           {
               if (AccountsPwInpPut.contentType == InputField.ContentType.Standard)
               {
                   AccountsPwInpPut.contentType = InputField.ContentType.Password;
                   AccountsPwInpPut.enabled = false;
                   AccountsPwInpPut.enabled = true;
               }
               else
               {
                   AccountsPwInpPut.contentType = InputField.ContentType.Standard;
                   AccountsPwInpPut.enabled = false;
                   AccountsPwInpPut.enabled = true;
               }
           }));
        UserType.onValueChanged.AddListener((bool args) =>
        {

            UserType.isOn = args;
            accountsGo.gameObject.SetActive(args);
            if (args)
            {
                AccountsphoneInput.text = "";
                AccountsPwInpPut.text = "";
            }
        });

        #endregion

        #region   手机验证码响应事件

        SMSSUserSMSSLogin.onClick.AddListener((() =>
        {
            if (!LoginUIController.Instance.VerifyAgreeToggle())
            {
                return;
            }
            if (LoginUIController.Instance.VerifyPhoneNo(SMSSphoneInput.text) && LoginUIController.Instance.VerifySMSCode(SMSSPwInpPut.text))
            {
                HttpManager.Instance.Login_SMSS(SMSSphoneInput.text, SMSSPwInpPut.text, (LoginUIController.Instance.PopupInfo));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));
        SMSSgetSMSSBtn.onClick.AddListener((() =>
        {
            if (LoginUIController.Instance.VerifyPhoneNo(SMSSphoneInput.text))
            {
             LoginUIController.Instance.FreezeButton(SMSSgetSMSSBtn);
                HttpManager.Instance.GetSMSS(SMSSphoneInput.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));
        SMSSType.onValueChanged.AddListener((bool args) =>
        {

            SMSSType.isOn = args;
            smssGo.gameObject.SetActive(args);

            if (args)
            {
                SMSSphoneInput.text = "";
                SMSSPwInpPut.text = "";
            }
        });
        #endregion
        #region   游客登录


        visitorBtn.onClick.AddListener(()=> {
       
            GlobalParameter.isVisitor = true;
            HttpManager.Instance.VisitorLogin(VisitorSuccess);

       
        });

        #endregion


        Init();

    }
    /// <summary>
    /// 游客登录成功
    /// </summary>
    /// <param name="obj"></param>
    private void VisitorSuccess(string obj)
    {
        if (obj=="200")
        {
            //提示成功
            //    LoginUIController.Instance.PopupInfo("200");

            //场景跳转
            SceneManager.LoadScene("main");



        }
    }

    private void Init()
    {
        accountsGo.gameObject.SetActive(true);
        UserType.isOn = true;


        smssGo.gameObject.SetActive(false);
        SMSSType.isOn = false;
    }








}
