﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgressPageUIPanel : UIWindowsBase {




    public Button backBtn;
    public override void Awake()
    {
        base.Awake();

        backBtn.onClick.AddListener(()=> {


            LoginUIController.Instance.SetNextUIState(LoginUIState.LoginPanel);
            LoginUIController.Instance.ShowUIState(LoginUIState.ProtocolPanel); 

        });


    }
}
