using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinDingPhonePanel : UIWindowsBase {


    public InputField phoneInput;
    public InputField smssInpPut;
    public Button getsmssBtn;
    public Button SetPhoneBtn;
    public Button backBtn;
    LoginPanelUI loginPanelUI;

    // Use this for initialization
    void Start () {
        loginPanelUI = GameObject.FindObjectOfType<LoginPanelUI>();
        backBtn.onClick.AddListener(() =>
        {
            LoginUIController.Instance.SetNextUIState(LoginUIState.LoginPanel);
        });
        getsmssBtn.onClick.AddListener((() =>
        {
            if (LoginUIController.Instance.VerifyPhoneNo(phoneInput.text))
            {
                LoginUIController.Instance.FreezeButton(getsmssBtn);
                HttpManager.Instance.GetSMSS(smssInpPut.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));
        SetPhoneBtn.onClick.AddListener((() =>
        {
            if (LoginUIController.Instance.VerifyPhoneNo(phoneInput.text) && LoginUIController.Instance.VerifySMSCode(smssInpPut.text))
            {
                HttpManager.Instance.DownloadTexture(loginPanelUI.UserIcon, "UserIcon.png", PublicAttribute.LocalFilePath + "APP/", (bytes
                        =>
                {
                    Debug.Log("头像下载成功");
                    HttpManager.Instance.BindingPhoneNo(phoneInput.text, smssInpPut.text, loginPanelUI.ThirdOpenID, bytes, loginPanelUI.UserName, (LoginUIController.Instance.PopupInfo));
                }));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的手机号");
            }
        }));

        this.gameObject.SetActive(false);
	}
	

}
