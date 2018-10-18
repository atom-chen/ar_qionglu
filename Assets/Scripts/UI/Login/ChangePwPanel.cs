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
           
                LoginUIController.Instance.FreezeButton(getSmssBtn);
                HttpManager.Instance.GetSMSS(PublicAttribute.UserName, (b =>
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
      if (LoginUIController.Instance.VerifyPwd(newpwInput.text) && LoginUIController.Instance.VerifyPwd(newpwRepeatInput.text)&&newpwInput.text==newpwRepeatInput.text&&LoginUIController.Instance.VerifySMSCode(smssInput.text))
            {
                HttpManager.Instance.ResetPwd(PublicAttribute.UserName, newpwRepeatInput.text, smssInput.text, (LoginUIController.Instance.PopupInfo));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的格式");
            }
        });




        this.gameObject.SetActive(false);
    }


}
