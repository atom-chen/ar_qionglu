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
            if (string.IsNullOrEmpty(phoneInput.text) || phoneInput.text.Length != 11)
            {
                LoginUIController.Instance.ShowPopup("", GlobalParameter.InputPhoneNumber);
                return;
            }
            if (LoginUIController.Instance.VerifyPhoneNo(phoneInput.text))
            {
                LoginUIController.Instance.FreezeButton(getsmssBtn);
                HttpManager.Instance.GetSMSS(phoneInput.text, (b =>
                {
                    Debug.Log("获取短信验证码 " + b);
                }));
            }
        
        }));
        SetPhoneBtn.onClick.AddListener((() =>
        {
            if (LoginUIController.Instance.VerifyPhoneNo(phoneInput.text) && LoginUIController.Instance.VerifySMSCode(smssInpPut.text))
            {
                HttpManager.Instance.DownloadTexture(GlobalParameter.UserIcon, "UserIcon.png", PublicAttribute.LocalFilePath + "APP/", (bytes
                        =>
                {
                    Debug.Log("头像下载成功");
                    HttpManager.Instance.BindingPhoneNo(phoneInput.text, smssInpPut.text, GlobalParameter.ThirduserID, bytes, GlobalParameter.UserName, (LoginUIController.Instance.PopupInfo));
                }));
            }
      
        }));

        this.gameObject.SetActive(false);
	}
	

}
