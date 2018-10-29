using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanelUI : UIWindowsBase
{


    public Button backBtn;
    public Button getSmssBtn;

    public Button registerBtn;

    public InputField phoneNoInput;
    public InputField smssInput;
    public InputField pwInput;
    public override void Awake()
    {
        base.Awake();


        backBtn.onClick.AddListener(() => {

            LoginUIController.Instance.SetNextUIState(LoginUIState.LoginPanel);


        });
        getSmssBtn.onClick.AddListener((() =>
        {
            Debug.Log("获取短信验证码 ");
            if (LoginUIController.Instance.VerifyPhoneNo(phoneNoInput.text))
            {
                LoginUIController.Instance.FreezeButton(getSmssBtn);
                HttpManager.Instance.GetSMSS(phoneNoInput.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));


        registerBtn.onClick.AddListener((() =>
        {
            if (string.IsNullOrEmpty(phoneNoInput.text))
            {
                LoginUIController.Instance.ShowPopup("", "请输入手机号");
            }
            else if (string.IsNullOrEmpty(pwInput.text))
            {
                LoginUIController.Instance.ShowPopup("", "请输入密码");
            }
            else if (string.IsNullOrEmpty(smssInput.text))
            {
                LoginUIController.Instance.ShowPopup("", "请输入验证码");
            }
            else     if (LoginUIController.Instance.VerifyPhoneNo(phoneNoInput.text) && LoginUIController.Instance.VerifyPwd(pwInput.text) && LoginUIController.Instance.VerifySMSCode(smssInput.text))
            {
                HttpManager.Instance.Register(phoneNoInput.text, pwInput.text, smssInput.text, (LoginUIController.Instance.PopupInfo));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));


        this.gameObject.SetActive(false);
    }


}
