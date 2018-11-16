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
            
                LoginUIController.Instance.ShowPopup(GlobalParameter.WrongFormat, GlobalParameter.InputPhoneNumber);
            }
        }));


        registerBtn.onClick.AddListener((() =>
        {
            if (LoginUIController.Instance.CheckInpuFormat(phoneNoInput.text, pwInput.text, smssInput.text))
                HttpManager.Instance.Register(phoneNoInput.text, pwInput.text, smssInput.text, (LoginUIController.Instance.PopupInfo));
           
   
        }));


        this.gameObject.SetActive(false);
    }


}
