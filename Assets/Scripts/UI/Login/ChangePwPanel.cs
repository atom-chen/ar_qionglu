using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePwPanel : UIWindowsBase


{

    public Button backBtn;

    public Button getSmssBtn;
    public Button sureBtn;

    public InputField newpwRepeatInput;
    public InputField newpwInput;
    public InputField smssInput;
    public override void Awake()
    {
        base.Awake();

        getSmssBtn.onClick.AddListener((() =>
        {
           
                FreezeButton(getSmssBtn);
                HttpManager.Instance.GetSMSS(PublicAttribute.UserInfo.PhoneNo, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
           
        }));
        backBtn.onClick.AddListener(() =>
        {
            LoginUIController.Instance.SetNextUIState(LoginUIState.LoginPanel);
        });
        sureBtn.onClick.AddListener(() =>
        {

            if (string.IsNullOrEmpty(newpwInput.text)|| newpwInput.text.Length!=6)
            {
                PP.ShowPopup("",GlobalParameter.InputPassword);
                return;
            }
            if (string.IsNullOrEmpty(newpwRepeatInput.text) || newpwRepeatInput.text.Length != 6)
            {
                PP.ShowPopup("", GlobalParameter.InputRepeatPassword);
                return;
            }
            if (newpwRepeatInput.text!= newpwInput.text)
            {
                PP.ShowPopup("", GlobalParameter.InputRepeatWrong);
                return;
            }
            if (string.IsNullOrEmpty(smssInput.text) || smssInput.text.Length != 6)
            {
                PP.ShowPopup("", GlobalParameter.InputSMSS);
                return;
            }




            if (VerifyPwd(newpwInput.text) && VerifyPwd(newpwRepeatInput.text)&&newpwInput.text==newpwRepeatInput.text&&VerifySMSCode(smssInput.text))
            {
                HttpManager.Instance.ResetPwd(PublicAttribute.UserInfo.PhoneNo, newpwRepeatInput.text, smssInput.text, (PopupInfo));
            }
    
        });



        this.gameObject.SetActive(false);
    }
    // 登陆界面弹出提示框
    public LoginUIPopupPage PP;
    /// <summary>
    /// 根据状态码执行
    /// </summary>
    /// <param name="status"></param>
    public void PopupInfo(string status)
    {
        Debug.Log(status);
        switch (status)
        {
            case "200":
                PP.ShowPopup("请求成功", "密码修改成功");
                gameObject.SetActive(false);
                break;
            case "300":
                PP.ShowPopup("意见提交成功", "意见提交成功，我们会尽快查看！");
                break;
            case "500":
                PP.ShowPopup("请求失败", "请求失败，请稍后再试！");
                break;
            case "Error":
                PP.ShowPopup("请求失败", "请稍候重试");
                break;
            case "null":
                PP.ShowPopup("格式错误", "昵称不可为空");
                break;
            case "1002":
                PP.ShowPopup("号码出错", "号码已存在，请更换手机号再试");
                break;
            
            case "1004":
                PP.ShowPopup("验证码错误", "请输入正确的验证码");
                break;
            case "1007":
                PP.ShowPopup("", "密码修改成功");
                break;
            default:
                PP.ShowPopup("请求失败", "请稍候重试");
                break;
        }
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
    private void FreezeButton(Button btn, float time = 60)
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

}
