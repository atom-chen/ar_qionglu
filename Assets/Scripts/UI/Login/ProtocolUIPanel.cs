using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtocolUIPanel : UIWindowsBase {

    public Button protocolBtn;


    public Text textLabel;
    public GameObject AgressPagePanel;
    public Toggle aggreeToggle;


    public override void Awake()
    {
        base.Awake();
        protocolBtn.onClick.AddListener(() =>
        {
            LoginUIController.Instance.SetNextUIState(LoginUIState.AgressPagePanel);

        });
        aggreeToggle.onValueChanged.AddListener(SetAlpha);
        AgressPagePanel.gameObject.SetActive(false);
    }

    private void SetAlpha(bool arg0)
    {
        if (arg0==false)
        {
            textLabel.color = new UnityEngine.Color(0f,0f,0f, 0.5f);
        }
        else
        {
            textLabel.color = new UnityEngine.Color(0f, 0f, 0f, 1f);
        }
    }
}
