using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowsBase : SingletonMono<UIWindowsBase> {

    public override void Awake()
    {
        base.Awake();






        ClearInputFieldText();
    }

    public void ClearInputFieldText()
    {
      


 InputField       [] inputTexts = transform.GetComponentsInChildren<InputField>();
        if (inputTexts.Length!=0)
        {
            foreach (var item in inputTexts)
            {
                item.text = "";
            }
        }
 
    }
}
