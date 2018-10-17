using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ForgetPwPanel : MonoBehaviour
{

    public Button backBtn;
    public Button getSmssBtn;

    public Button sureBtn;


    public InputField phoneNoInput;
    public InputField smssInput;
    public InputField pwInput;

    private void Awake()
    {
        backBtn.onClick.AddListener(() =>
        {
            LoginUIController.Instance.SetNextUIState(LoginUIState.LoginPanel);
        });

        getSmssBtn.onClick.AddListener((() =>
        {
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
        sureBtn.onClick.AddListener((() =>
        {
            if (LoginUIController.Instance.VerifyPhoneNo(phoneNoInput.text) && LoginUIController.Instance.VerifyPwd(pwInput.text) && LoginUIController.Instance.VerifySMSCode(smssInput.text))
            {
                HttpManager.Instance.ResetPwd(phoneNoInput.text, pwInput.text, smssInput.text, (LoginUIController.Instance.PopupInfo));
            }
            else
            {
                LoginUIController.Instance.ShowPopup("格式不正确", "请输入正确的格式");
            }
        }));




        this.gameObject.SetActive(false);
    }
}
