using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIPopupPage : MonoBehaviour
{
    public GameObject SingleTipGo;
    public GameObject TwiceTipGo;
    public Button ColliderBtn1,ColliderBtn2;
    public Text TitleText;
    public Text ContentTextSingle,ContentTextTwice;

    void Awake()
    {
        Hide();
        ColliderBtn1.onClick.AddListener(Hide);
        ColliderBtn2.onClick.AddListener(Hide);
    }

    /// <summary>
    /// 弹框提示信息，分两类
    /// ----第一类：两行内容的=== title：标题， content：内容----
    /// 第二类：一行内容的=== title为空，     content：内容----
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    public void ShowPopup(string title, string content)
    {

        if (string.IsNullOrEmpty(title) && content != null)
        {
        ContentTextSingle.text = content;
            SingleTipGo.SetActive(true);
            if (content.Length<14)
            {
                  ContentTextSingle.alignment = TextAnchor.MiddleCenter;
            }
            else
            {
                ContentTextSingle.alignment = TextAnchor.MiddleLeft;
            }
          


        }
        else
        {
            TitleText.text = title;
            ContentTextTwice.text = content;
            TwiceTipGo.SetActive(true);

        }
      CoroutineWrapper.EXES(4f, () => { Hide(); });
    }
    /// <summary>
    /// 显示弹出窗并执行函数
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="delayTime"></param>
    /// <param name="callback"></param>
    public void ShowPopupAndDoSomeThing(string title, string content, float delayTime, Action callback)
    {
        ShowPopup(title,content);
        var guid = Guid.NewGuid();
        TimerManager.Instance.Register(guid, delayTime, (() =>
        {
            Hide();
            TimerManager.Instance.UnRegister(guid);
            callback();
        }));
    }


    public void Hide()
    {
        if (SingleTipGo != null)
        {
            SingleTipGo.SetActive(false);
        }
        if (TwiceTipGo != null)
        {
            TwiceTipGo.SetActive(false);
        }
    }
    public void UpdateCheck()
    {
        ShowPopup("","当前版本已为最新版本");
    }
  
}
