﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUIController : SingletonMono<LoginUIController>
{
    [Header("所有的UIPanels")]
    public GameObject[] uiPanels;



    //public GameObject RegistPanel;
    //public GameObject ResetPwdPanel;
    //public GameObject ChangePwPanel;
    //public GameObject AgressPage;
    //public GameObject BinDingPhonePage;


    public Toggle AgreeToggle;
    public LoginUIPopupPage tipPanel;



    public override void Awake()
    {
        base.Awake();

        AgreeToggle.isOn = PlayerPrefs.HasKey(GlobalParameter.isHasRegister);

        if (CheckNetWork() ==false)
      {
            tipPanel.ShowPopup("网络错误", "请检查是否连接到网络");

        }


    }

    public bool CheckNetWork()
    {
        if (Application.internetReachability==NetworkReachability.NotReachable)
        {
            return false;
        }
        else
            return true;
    
    }

    /// <summary>
    /// 设置下一个要显示的UIPanel所在的LoginUIState
    /// </summary>
    /// <param name="loginUIState"></param>
    public void SetNextUIState(LoginUIState loginUIState)
    {
        SetState(loginUIState.ToString());


    }
    private void SetState(string   uiName)
    {
        foreach (var item in uiPanels)
        {
            if (item.name==uiName)
            {
                item.SetActive(true);
                item.GetComponent<UIWindowsBase>().ClearInputFieldText();
            }
            else
            {
          
             
      item.SetActive(false);

            }
        }
    }
    internal void HideCurrentUIState(string name)
    {
        foreach (var item in uiPanels)
        {
            if (item.name == name)
            {

                item.SetActive(false);
                item.GetComponent<UIWindowsBase>().ClearInputFieldText();
            }
        }
    }
    internal void ShowUIState(LoginUIState loginUIState)
    {
        foreach (var item in uiPanels)
        {
            if (item.name == loginUIState.ToString())
            {

                item.SetActive(true);
                
            }
        }
    }

    #region   用户协议


    /// <summary>
    /// 是否同意了用户协议
    /// </summary>
    /// <returns></returns>
    public bool VerifyAgreeToggle()
    {
        if (!AgreeToggle.isOn)
        {
        //    tipPanel.ShowPopup("用户协议", "同意用户协议才能登录");
            return false;
        }
        return true;
    }



    public void PopupInfo(string status)
    {
        GlobalParameter.isVisitor = false;
            //不是游客登录
        Debug.Log(status);
        switch (status)
        {
            case "200":
                tipPanel.ShowPopup("请求成功", "请求成功");
                break;
            case "1000":
                tipPanel.ShowPopup("账号或密码错误", "请输入正确的账号或密码");
                break;
            case "1002":
                tipPanel.ShowPopup("手机号已被注册", "请直接使用该手机号登录");
                break;
            case "1004":
                tipPanel.ShowPopup("验证码错误", "请输入正确的验证码");
                break;
            case "1005":

                tipPanel.ShowPopup("手机号格式错误", "请输入正确的手机号码");
                break;
            case "1006":
                //注册成功
                tipPanel.ShowPopupAndDoSomeThing(GlobalParameter.RegisterSuccessTitle, GlobalParameter.RegisterSuccessMsgs, 1.5f, LoadMainScene);
                //tipPanel.ShowPopup("操作成功", "请妥善保管账号和密码");
                break;
            case "1007":
                //密码修改
                tipPanel.ShowPopupAndDoSomeThing("密码修改成功", "请妥善保管账号和密码", 1.5f, InitState);
                break;
            case "1010":
                tipPanel.ShowPopupAndDoSomeThing("", "登录成功", 1.5f, LoadMainScene);
   
                break;
            case "1011":
                tipPanel.ShowPopup("未设置密码", "密码未设置，请尽快完善密码");
                break;
            case "1012":
                tipPanel.ShowPopupAndDoSomeThing("密码已设置", "请妥善保管账号和密码", 1.5f, InitState);
                break;
            case "1013":
                tipPanel.ShowPopup("未绑定手机号", "请绑定手机号");
                break;
            case "1014":
                tipPanel.ShowPopup("", "该手机号并未注册");
                break;
            case "Error":
                tipPanel.ShowPopup("登录失败", "请稍候重试");
                break;
            case "Third":
                tipPanel.ShowPopupAndDoSomeThing("", "登录成功", 1.5f, LoadMainScene);
                break;
            default:
                break;
        

        }


    }

    private static void LoadMainScene()
    {
        UnityHelper.LoadNextScene("main");

        PublicAttribute.UserInfo = new UserInfo()
        {
            PhoneNo = GlobalParameter.nickName,
            NickName = GlobalParameter.phone,
            UserIcon = null,
        };

    
        PlayerPrefs.SetInt(GlobalParameter.isHasRegister, 6666);
    }
    /// <summary>
    /// 弹框提示信息，分两类
    /// ----第一类：两行内容的=== title：标题， content：内容----
    /// 第二类：一行内容的=== title为空，     content：内容----
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    public void ShowPopup(string title, string content)
    {
        tipPanel.ShowPopup(title, content);
    }

    private void InitState()
    {
        SetNextUIState(LoginUIState.LoginPanel);
    }
    #endregion



    public void FreezeButton(Button btn, float time = 60)
    {
        Text text = btn.gameObject.GetComponentInChildren<Text>();
        string oldText = text.text;

        StartCoroutine(changeTime(btn, text, time, oldText));
    }
    IEnumerator changeTime(Button btn, Text text, float time, string oldText)
    {
        btn.interactable = false;
        while (time > 0)
        {
            text.text = time + "s";
            //暂停一秒
            yield return new WaitForSeconds(1);
            time--;
        }
        btn.interactable = true;
        text.text = oldText;
    }


    #region    输入验证区域


    /// <summary>
    /// 验证手机号是否符合格式
    /// </summary>
 public   bool VerifyPhoneNo(string str)
    {
 return     System.Text.RegularExpressions.Regex.IsMatch(str, @"^[1]+[3,5,7,8]+\d{9}");
    }
    /// <summary>
    /// 验证密码是否符合格式
    /// </summary>
  public  bool VerifyPwd(string str)
    {
    return    System.Text.RegularExpressions.Regex.IsMatch(str,@"^\d{6,21}$");
      
    }

    /// <summary>
    /// 验证短信验证码格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
 public   bool VerifySMSCode(string str)
    {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\d{6}");

    }
    /// <summary>
    /// 验证输入的格式是否正确
    /// -2、验证网络
    /// -1、验证协议
    /// 1、phoneNum=手机号 ；
    /// 2、password=密码；
    /// 3、password=短信验证码
    /// </summary>
    /// <param name="phoneNum"></param>
    /// <param name="password"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool CheckInpuFormat(string  phoneNum,string   password=null,string   smssCode=null)
    {
        if (CheckNetWork() == false)
        {

           ShowPopup("网络错误", "请检查是否连接到网络");
            return false;
        }
        if (VerifyAgreeToggle() == false)
        {

           ShowPopup("", GlobalParameter.AgrrToggle);
            return false;
        }

        if (VerifyPhoneNo(phoneNum) == false)
        {
     ShowPopup(GlobalParameter.WrongFormat, GlobalParameter.InputPhoneNumber);
            return false;
        }
        if (!string.IsNullOrEmpty(password))
        {
            if (VerifyPwd(password) == false)
            {
                ShowPopup(GlobalParameter.WrongFormat, GlobalParameter.InputPassword);
                return false;
            }
        }


        if (!string.IsNullOrEmpty(smssCode))
        {
            if (VerifySMSCode(smssCode) == false)
            {
                ShowPopup(GlobalParameter.WrongFormat, GlobalParameter.InputSMSS);
                return false;
            }
        }
  
        return true;
    }







    #endregion



}
