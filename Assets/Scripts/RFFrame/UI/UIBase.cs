/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     UIBase.cs 
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

public abstract class UIBase :MonoBehaviour
{
    /// <summary>
    /// 根节点
    /// </summary>
    public Transform RootTransform;
    /// <summary>
    /// 获取本窗口的ID
    /// </summary>
    public UIID Uiid = UIID.Empty;
    /// <summary>
    /// 窗口的层级
    /// Index越大，层级在最前面
    /// </summary>
    public int SiblingIndex = 5;

    /// <summary>
    /// 是否可以同时出现多个
    /// </summary>
    public bool Repeat = false;
    /// <summary>
    /// 永不重复的ID号  用于多个控件时区分 
    ///  </summary>
    public string GUID;

    /// <summary>
    /// 设置是否显示隐藏
    /// </summary>
    /// <param name="state"></param>
    /// <param name="action"></param>
    public virtual void SetState(bool state, Action action = null)
    {
        Utility.SetGoState(this.gameObject, state);
        if (action != null)
        {
            action();
        }
    }

    /// <summary>
    /// 销毁界面
    /// </summary>
    /// <param name="action">销毁时的反应</param>
    public virtual void DestroyUI(Action action = null)
    {
        if (action != null)
        {
            action();
        }
        Destroy(gameObject);
    }
    /// <summary>
    /// 界面显示
    /// </summary>
    public virtual void OnEnter()
    {

    }
    /// <summary>
    /// 界面暂停
    /// 弹出了其他的页面
    /// </summary>
    public virtual void OnPause()
    {

    }
    /// <summary>
    /// 界面继续
    /// 其他界面被移除，恢复本页的交互功能
    /// </summary>
    public virtual void OnResume()
    {

    }
    /// <summary>
    /// 界面被移除
    /// </summary>
    public virtual void OnExit()
    {

    }
}
