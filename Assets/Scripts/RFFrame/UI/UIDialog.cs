/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     Dialog.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-25 
 *Description:    
 *History: 
*/

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialog : UIBase 
{
    public enum MessageBoxButtons
    {
        OK,
        OKCancel
    }

    #region 属性
    public Button YesBtn;
    public Button NoBtn;
    public Button CloseBtn;
    public Text TitleText;
    public Text ContentText;

    #endregion

    #region 私有
    private void Awake()
    {
        CloseBtn.onClick.AddListener(CloseSelf);
        // Y  = 121   KeypadEnter =271  Return = 13,
        HardWareManager.Instance.AddHandler(HardWareEventType.KeyDown, 121, Affirm);
        HardWareManager.Instance.AddHandler(HardWareEventType.KeyDown, 271, Affirm);
        HardWareManager.Instance.AddHandler(HardWareEventType.KeyDown, 13, Affirm);

        //Escape = 27  N = 110,
        HardWareManager.Instance.AddHandler(HardWareEventType.KeyDown, 27, Cancel);
        HardWareManager.Instance.AddHandler(HardWareEventType.KeyDown, 110, CloseSelf);
    }

    private void Cancel()
    {
        NoBtn.onClick.Invoke();
    }

    private void Affirm()
    {
        YesBtn.onClick.Invoke();
    }

    private void CloseSelf()
    {
        UIManager.Instance.DestroyWindow(this.Uiid);
    }

    private void OnDestroy()
    {
        HardWareManager.Instance.RemoveHandler(HardWareEventType.KeyDown, 121, Affirm);
        HardWareManager.Instance.RemoveHandler(HardWareEventType.KeyDown, 271, Affirm);
        HardWareManager.Instance.RemoveHandler(HardWareEventType.KeyDown, 13, Affirm);
        HardWareManager.Instance.RemoveHandler(HardWareEventType.KeyDown, 27, Cancel);
        HardWareManager.Instance.RemoveHandler(HardWareEventType.KeyDown, 110, CloseSelf);
    }

    #endregion

    #region Public
    public virtual void ShowDialog(Action<bool> callback, MessageBoxButtons btn = MessageBoxButtons.OKCancel)
    {
        if (btn == MessageBoxButtons.OK)
        {
            NoBtn.gameObject.SetActive(false);
            YesBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(24, 0);
        }

        YesBtn.onClick.AddListener((() =>
        {
            callback(true);
            CloseSelf();
        }));
        NoBtn.onClick.AddListener((() =>
        {
            callback(false);
            CloseSelf();
        }));
    }

    #endregion
}
