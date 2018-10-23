using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIPopupPage : MonoBehaviour
{
    public GameObject TipGo;
    public Button ColliderBtn;
    public Text TitleText;
    public Text ContentText;

    void Awake()
    {
        Hide();
        ColliderBtn.onClick.AddListener(Hide);
    }


    public void ShowPopup(string title, string content)
    {
        TitleText.text = title;
        ContentText.text = content;
        TipGo.SetActive(true);
        CoroutineWrapper.EXES(2f, () => { Hide(); });
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
        TipGo.SetActive(false);
    }


}
