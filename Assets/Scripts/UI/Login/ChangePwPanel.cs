using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePwPanel : MonoBehaviour


{

    public Button backBtn;


    public Button sureBtn;

    public InputField oldpwInput;
    public InputField newpwInput;
    private void Awake()
    {
        backBtn.onClick.AddListener(() =>
        {
            LoginUIController.Instance.SetNextUIState(LoginUIState.LoginPanel);
        });
        this.gameObject.SetActive(false);
    }


}
