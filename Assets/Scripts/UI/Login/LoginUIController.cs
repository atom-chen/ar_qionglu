using System;
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
            }
            else
            {
                item.SetActive(false);
                item.GetComponent<UIWindowsBase>().ClearInputFieldText();
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
        PublicAttribute.isVisitor = false;
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
                tipPanel.ShowPopupAndDoSomeThing("操作成功", "请妥善保管账号和密码", 1.5f, InitState);
                //tipPanel.ShowPopup("操作成功", "请妥善保管账号和密码");
                break;
            case "1007":
                tipPanel.ShowPopupAndDoSomeThing("密码修改成功", "请妥善保管账号和密码", 1.5f, InitState);
                break;
            case "1010":
                ScenesManager.Instance.LoadMainScene();
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
            case "Error":
                tipPanel.ShowPopup("请求失败", "请稍候重试");
                break;
            default:
                break;
        

        }


    }
    public void ShowPopup(string title, string content)
    {
        tipPanel.ShowPopup(title, content);
    }

    private void InitState()
    {
        SetNextUIState(LoginUIState.LoginPanel);
    }
    #endregion



    public void FreezeButton(Button btn, float time = 15)
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
            text.text = time + "";
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
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length != 11)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 验证密码是否符合格式
    /// </summary>
  public  bool VerifyPwd(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length < 6 || str.Length > 21)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 验证短信验证码格式
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
 public   bool VerifySMSCode(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        if (str.Length != 6)
        {
            return false;
        }
        return true;
    }
    #endregion
}
