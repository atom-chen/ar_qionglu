/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     EventManager.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-24 
 *Description:    
 *History: 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventManager : Singleton<EventManager>
{

    public delegate void GlobalEventHandler(GlobalEventType eventType, params object[] args);

    private HashSet<GlobalEventHandler>[] _globalHandler;

    public EventManager()
    {
        _globalHandler = new HashSet<GlobalEventHandler>[(int)GlobalEventType.End];
        for (int i = 0; i < _globalHandler.Length; i++)
        {
            _globalHandler[i] = new HashSet<GlobalEventHandler>();
        }
    }

    /// <summary>
    /// 添加事件回调函数
    /// </summary>
    /// <param name="handler">回调处理函数</param>
    /// <param name="events">事件类型</param>
    public void AddHandler(GlobalEventHandler handler, params GlobalEventType[] events)
    {
        foreach (GlobalEventType type in events)
        {
            _globalHandler[(int)type].Add(handler);
        }
    }

    /// <summary>
    /// 删除事件回调函数
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="handler">指定函数</param>
    public void RemoveHandler(GlobalEventHandler handler, GlobalEventType type)
    {
        _globalHandler[(int)type].Remove(handler);
    }

    /// <summary>
    /// 清空指定类型的事件回调函数
    /// </summary>
    /// <param name="type"></param>
    public void ClearCallBack(GlobalEventType type)
    {
        _globalHandler[(int)type].Clear();
    }

    /// <summary>
    /// 清空所有类型的事件
    /// </summary>
    public void ClearAllHandler()
    {
        for (int i = 0; i < _globalHandler.Length; i++)
        {
            this.ClearCallBack((GlobalEventType)i);
        }
    }
    /// <summary>
    /// 执行指定类型的事件
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="args">参数</param>
    public void SendEvent(GlobalEventType type, params object[] args)
    {
        var handlers = _globalHandler[(int)type].ToArray();
        for (int i = 0; i < handlers.Length; i++)
        {
            var t = handlers[i];
            t(type, args);
        }
    }
}
