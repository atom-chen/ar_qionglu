/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     UIManager.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-24 
 *Description:    
 *History: 
*/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UIManager : Singleton<UIManager> {


    #region 属性
    private GameObject CavasGo;

    private UIBase _currentItem;
    /// <summary>
    /// 当前界面中被选中的项
    /// </summary>
    public UIBase CurrentItem
    {
        get { return _currentItem; }
        set { _currentItem = value; }
    }

    #region 保存实例化出来的界面组件
    private Dictionary<UIID, UIBase> _allWindowsByUIID;

    /// <summary>
    /// 保存所有实例化的窗口资源
    /// </summary>
    /// 
    protected Dictionary<UIID, UIBase> AllWindowsByUiidByUiid
    {
        get
        {
            if (_allWindowsByUIID == null)
            {
                _allWindowsByUIID = new Dictionary<UIID, UIBase>();
            }
            return _allWindowsByUIID;
        }
    }

    private Dictionary<string, UIBase> _allWindowsByGUID;

    /// <summary>
    /// 保存所有实例化的窗口资源
    /// </summary>
    /// 
    protected Dictionary<string, UIBase> AllWindowsByUiidByGUID
    {
        get
        {
            if (_allWindowsByGUID == null)
            {
                _allWindowsByGUID = new Dictionary<string, UIBase>();
            }
            return _allWindowsByGUID;
        }
    }


    #endregion

    #endregion

    #region 私有方法
    protected override void Init()
    {
        base.Init();
        CavasGo = GameObject.Find("Canvas");
    }

    #endregion

    #region 公共方法

    /// <summary>
    /// 通过ID获取UIBase
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UIBase GetUIBase(UIID id)
    {
        if (_allWindowsByUIID.ContainsKey(id))
        {
            return _allWindowsByUIID[id];
        }
        return null;
    }


    public UIBase GetUIBase(string  GUID)
    {
        if (_allWindowsByGUID.ContainsKey(GUID))
        {
            return _allWindowsByGUID[GUID];
        }
        return null;
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <param name="action"></param>
    public void SetState(UIID id,bool state, Action action = null)
    {
        var ui =  GetUIBase(id);
        ui.SetState(state, action);

    }
    public void SetState(string UIGD, bool state, Action action = null)
    {
        var ui = GetUIBase(UIGD);
        ui.SetState(state, action);
    }

    /// <summary>
    /// 销毁UI
    /// </summary>
    /// <param name="id">UIID</param>
    public void DestroyWindow(UIID id,string GUID = null,Action action = null)
    {
        UIBase ui;
        if (GUID == null )
        {
            ui = GetUIBase(id);
        }
        else
        {
            ui = GetUIBase(GUID);
        }
        if (ui != null)
        {
            if (action != null )
            {
                action();
            }
            ui.DestroyUI();
            if (_allWindowsByUIID != null) _allWindowsByUIID.Remove(id);
            if (_allWindowsByGUID != null) _allWindowsByGUID.Remove(GUID);
        }
        CurrentItem = null;
    }
    /// <summary>
    /// 销毁所有的界面
    /// </summary>
    public void DestroyAllWindows()
    {
        if (_allWindowsByUIID != null && _allWindowsByUIID.Count > 0)
        {
            foreach (KeyValuePair<UIID, UIBase> window in _allWindowsByUIID)
            {
                UIBase baseWindow = window.Value;
                baseWindow.DestroyUI();
            }
            _allWindowsByUIID.Clear();
            _allWindowsByGUID.Clear();
            CurrentItem = null;
        }
    }
    /// <summary>
    /// 实例化UI
    /// </summary>
    /// <param name="id"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public UIBase InstanceWindow(UIID id,Action action = null)
    {
        UIBase UB = GetUIBase(id);
        if (_allWindowsByUIID.ContainsKey(id))
        {
            if (!UB.Repeat)
            {
                DestroyWindow(id);
            }
        }
        if (UIResource.WindowPrefabs.ContainsKey(id))
        {
            string prefabPath = UIResource.WindowPrefabs[id].Path;
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                GameObject Go = Utility.InstantiateGo(prefab, true);
                UB = Go.GetComponent<UIBase>();
                Utility.AddChildToTarget_UI(UB.RootTransform ?? CavasGo.transform, Go.transform, UB.SiblingIndex);
                UB.GUID = System.Guid.NewGuid().ToString();
            }
        }
        if (UB != null)
        {
            UB.SetState(true);
            _allWindowsByUIID[id] = UB;
            _allWindowsByGUID[UB.GUID] = UB;
            if (action != null )
            {
                action();
            }
        }
        return UB;
    }
    #endregion

}
