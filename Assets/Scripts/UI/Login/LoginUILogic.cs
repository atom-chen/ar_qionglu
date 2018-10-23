using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUILogic : MonoBehaviour
{

    /// <summary>
    /// �û��������½
    /// </summary>
    public Toggle LoginPage_UserPwdLoginToggle;
    /// <summary>
    /// ���ŵ�½
    /// </summary>
    public Toggle LoginPage_SmsLoginToggle;

    public InputField LoginPage_UserInputField;

    public InputField LoginPage_PwdInputField;


    public GameObject LoginPage_AccountsGo;

    public GameObject LoginPage_SMSSGo;
    void Awake()
    {
        LoginPage_UserPwdLoginToggle.onValueChanged.AddListener((arg0 =>
        {
            if (arg0)
            {
                LoginPage_UserInputField.text = "";
                LoginPage_PwdInputField.text = "";
            }
        } ));

        LoginPage_SmsLoginToggle.onValueChanged.AddListener((arg0 =>
        {
            if (arg0)
            {
                LoginPage_UserInputField.text = "";
                LoginPage_PwdInputField.text = "";
            }
        } ));
    }

}
