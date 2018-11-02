using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowsBase : SingletonMono<UIWindowsBase>
{
    [Header("当前面板中的登录注册类按钮")]
    public Button[] clickBtns;
   public List<InputField> inputFields;
    public override void Awake()
    {
        base.Awake();
        inputFields = new List<InputField>();
        ClearInputFieldText();
        DisableInteractive();
    }

    protected void DisableInteractive()
    {
   
        if (clickBtns.Length!=0)
        {
            foreach (var item in clickBtns)
            {
                item.interactable = false;
            }
        }
    }

    public void ClearInputFieldText()
    {
        inputFields.Clear();
 InputField       [] inputTexts = transform.GetComponentsInChildren<InputField>();
        if (inputTexts.Length!=0)
        {
            foreach (var item in inputTexts)
            {
                inputFields.Add(item);
                item.text = "";
            }
        }
        DisableInteractive();
    }

    public virtual void Update()
    {
        if (clickBtns.Length!=0&&clickBtns[0].gameObject!=null&&clickBtns[0].transform.parent.gameObject.activeSelf==true)
        {
            for (int i = 0; i < inputFields.Count; i++)
            {
                if (!string.IsNullOrEmpty(inputFields[i].text))
                {
           
                    if (i==inputFields.Count-1)
                    {

                        foreach (var btns in clickBtns)
                        {
                            btns.interactable = true;
                        }
                    }
                    else
                    {
                 
                        continue;
                    }
                }
                else
                {
                    foreach (var btns in clickBtns)
                    {
                        btns.interactable = false;
                    }
                    break;
                }
            }
         
        }
     
    }
}
