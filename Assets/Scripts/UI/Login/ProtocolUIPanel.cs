using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtocolUIPanel : UIWindowsBase {

    public Button protocolBtn;
    public GameObject AgressPagePanel;
    public override void Awake()
    {
        base.Awake();
        protocolBtn.onClick.AddListener(() =>
        {
            LoginUIController.Instance.SetNextUIState(LoginUIState.AgressPagePanel);

        });
        AgressPagePanel.gameObject.SetActive(false);
    }


 
}
