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



    public Toggle UserType;


    public List<InputField> accountInpueField;
    #endregion


    #region  SMSS

    [Header("手机验证码登录区域")]
    public GameObject smssGo;
    public InputField SMSSphoneInput;
    public InputField SMSSPwInpPut;
    public Button SMSSgetSMSSBtn;
    public Button SMSSUserSMSSLogin;


    public Toggle SMSSType;


    public List<InputField> smssInputField;
    #endregion

    private void InitField()
    {
        if (accountInpueField==null)
        {
            accountInpueField = new List<InputField>();

        }
        else
        {
      accountInpueField.Clear();
        }
  
        if (smssInputField == null)
        {    smssInputField = new List<InputField>();
        }
        else
        {    smssInputField.Clear();

        }
    
    
        foreach (var item in inputFields)
        {
            if (item.transform.parent.name == "Accounts")
            {
                accountInpueField.Add(item);
            }
            else
            {
                smssInputField.Add(item);
            }
        }
    }

    public override void Awake()
    {
        base.Awake();
        InitField();


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
            if (LoginUIController.Instance.VerifyAgreeToggle() == false)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.AgrrToggle);
                return;
            }
            if (string.IsNullOrEmpty(AccountsphoneInput.text) || AccountsphoneInput.text.Length != 11)
            {
                LoginUIController.Instance.ShowPopup("",GlobalParameter.InputPhoneNumber);
                return;
            }
             if (string.IsNullOrEmpty(AccountsPwInpPut.text) || AccountsPwInpPut.text.Length < 6)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.InputPassword);
                return;
            }
             if (LoginUIController.Instance.VerifyPhoneNo(AccountsphoneInput.text) && LoginUIController.Instance.VerifyPwd(AccountsPwInpPut.text))
            {
                HttpManager.Instance.Login_UserPwd(AccountsphoneInput.text, AccountsPwInpPut.text, (LoginUIController.Instance.PopupInfo));
            }
         
        }));

        UserType.onValueChanged.AddListener((bool args) =>
        {

            UserType.isOn = args;
            accountsGo.gameObject.SetActive(args);

            if (args)
            {
                if (accountInpueField == null)
                {
                    accountInpueField = new List<InputField>();
    accountInpueField.Add(AccountsphoneInput);
                    accountInpueField.Add(AccountsPwInpPut);
                }
                else if (accountInpueField.Count == 0)
                {
                    accountInpueField.Add(AccountsphoneInput);
                    accountInpueField.Add(AccountsPwInpPut);
                }
                AccountsphoneInput.text = "";
                AccountsPwInpPut.text = "";
            }
        });

        #endregion

        #region   手机验证码响应事件

        SMSSUserSMSSLogin.onClick.AddListener((() =>
        {
            if (LoginUIController.Instance.VerifyAgreeToggle() == false)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.AgrrToggle);
                return;
            }
            if (string.IsNullOrEmpty(SMSSphoneInput.text)|| SMSSphoneInput.text.Length!=11)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.InputPhoneNumber);
                return;
            }
             if (string.IsNullOrEmpty(SMSSPwInpPut.text) || SMSSPwInpPut.text.Length != 6)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.InputSMSS);
                return;
            }
            if (LoginUIController.Instance.VerifyPhoneNo(SMSSphoneInput.text) && LoginUIController.Instance.VerifySMSCode(SMSSPwInpPut.text))
            {
                HttpManager.Instance.Login_SMSS(SMSSphoneInput.text, SMSSPwInpPut.text, (LoginUIController.Instance.PopupInfo));
            }
           
        }));
        SMSSgetSMSSBtn.onClick.AddListener((() =>
        {
            if (string.IsNullOrEmpty(SMSSphoneInput.text) || SMSSphoneInput.text.Length != 11)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.InputPhoneNumber);
                return;
            }
            if (LoginUIController.Instance.VerifyPhoneNo(SMSSphoneInput.text))
            {
             LoginUIController.Instance.FreezeButton(SMSSgetSMSSBtn);
                HttpManager.Instance.GetSMSS(SMSSphoneInput.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
      
        }));
        SMSSType.onValueChanged.AddListener((bool args) =>
        {

            SMSSType.isOn = args;
            smssGo.gameObject.SetActive(args);

            if (args)
            {
                if (smssInputField == null)
                {
                    smssInputField = new List<InputField>();
                    smssInputField.Add(SMSSphoneInput);
                    smssInputField.Add(SMSSPwInpPut);
                }
                else if (smssInputField.Count==0)
                {
                    smssInputField.Add(SMSSphoneInput);
                    smssInputField.Add(SMSSPwInpPut);
                }
                
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

    public override void Update()
    {
        if (this.gameObject.activeSelf==true)
        {
            if (accountsGo.activeSelf)
            {
            for (int i = 0; i < accountInpueField.Count; i++)
            {
                if (!string.IsNullOrEmpty(accountInpueField[i].text))
                {
                    if (i == accountInpueField.Count - 1)
                    {
                            clickBtns[0].interactable = true;
                        }
                    else
                        {
                            continue;
                    }
                }
                else
                {
                        clickBtns[0].interactable = false;
                        break;
                }
            }
            }
            if (smssGo)
            {
                for (int i = 0; i < smssInputField.Count; i++)
                {
                    if (!string.IsNullOrEmpty(smssInputField[i].text))
                    {

                        if (i == smssInputField.Count - 1)
                        {
                  
                            clickBtns[1].interactable = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                      
                        clickBtns[1].interactable = false;
                        break;
                    }
                }
            }
      
        }

    }




}
