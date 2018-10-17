using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoginPanelUI : MonoBehaviour
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

    private void Awake()
    {
        ForgetPwdBtn.onClick.AddListener(() => {

            LoginUIController.Instance.SetNextUIState(LoginUIState.ForgetPwdPanel);

        });

        #region   账号密码响应事件

        registerBtn.onClick.AddListener((() =>
            {
                LoginUIController.Instance.SetNextUIState(LoginUIState.RegistPanel);
            }));

        AccountsUserPwdLogin.onClick.AddListener((() =>
        {
            if (!LoginUIController.Instance.VerifyAgreeToggle())
            {
                return;
            }

            if (LoginUIController.Instance.VerifyPhoneNo(AccountsphoneInput.text) && LoginUIController.Instance.VerifyPwd(AccountsPwInpPut.text))
            {
                HttpManager.Instance.Login_UserPwd(AccountsphoneInput.text, AccountsPwInpPut.text, (LoginUIController.Instance.PopupInfo));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));
        AccountseyeBtn.onClick.AddListener((() =>
           {
               if (AccountsPwInpPut.contentType == InputField.ContentType.Standard)
               {
                   AccountsPwInpPut.contentType = InputField.ContentType.Password;
               }
               else
               {
                   AccountsPwInpPut.contentType = InputField.ContentType.Standard;
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
             LoginUIController.Instance.   FreezeButton(SMSSgetSMSSBtn);
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



        Init();

    }

    private void Init()
    {
        accountsGo.gameObject.SetActive(true);
        UserType.isOn = true;


        smssGo.gameObject.SetActive(false);
        SMSSType.isOn = false;
    }








}
