/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     HardWareManager.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-25 
 *Description:    
 *History: 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public enum HardWareEventType
{
    KeyDown = 0,
    KeyUp,
    MouseDown,
    MouseUp,
    MouseMove,
    MouseScroll,
    MouseDBClick,
    MouseDrag,
    Count,
}

/// <summary>
/// 发送硬件消息的入口：
/// 键盘：每个键的按下和松开
/// 鼠标：鼠标的按下、松开、滚轮、移动
/// </summary>
public class HardWareManager : Singleton<HardWareManager>
{
    #region 字段
    private float _mouseMoveX = 0f;
    private float _mouseMoveY = 0f;
    private float _mouseScroll = 0f;
    private bool notClickUI;

    private SortedList<int, List<HardWareEventHandler>>[] _hardWareEvents;
    #endregion

    #region 属性
    private bool _lockKeyBoard;

    public bool LockKeyBoard
    {
        get { return _lockKeyBoard; }
        set { _lockKeyBoard = value; }
    }

    private bool _lockMouse;

    public bool LockMouse
    {
        get { return _lockMouse; }
        set { _lockMouse = value; }
    }


    public bool Lock { set { LockKeyBoard = LockMouse = value; } }

    public float MouseMoveX { get { return LockMouse ? 0f : _mouseMoveX; } }
    public float MouseMoveY { get { return LockMouse ? 0f : _mouseMoveY; } }
    public float MouseScroll { get { return LockMouse ? 0f : _mouseScroll; } }
    public bool GetKey(KeyCode code) { return LockKeyBoard ? false : Input.GetKey(code); }
    #endregion

    #region 事件
    public delegate void HardWareEventHandler();
    #endregion

    #region public method

    protected override void Init()
    {
        base.Init();
        _hardWareEvents = new SortedList<int, List<HardWareEventHandler>>[(int)HardWareEventType.Count];
        for (int i = 0; i < _hardWareEvents.Length; i++)
        {
            _hardWareEvents[i] = new SortedList<int, List<HardWareEventHandler>>();
        }
    }

    public void KeyboardLoop()
    {
        if (!LockKeyBoard)
        {
            // 键盘按下
            SortedList<int, List<HardWareEventHandler>> list = _hardWareEvents[(int)HardWareEventType.KeyDown];
            foreach (KeyValuePair<int, List<HardWareEventHandler>> item in list)
            {
                if (Input.GetKeyDown((KeyCode)item.Key))
                {
                    SendEvent(item.Value);
                }
            }

            // 键盘松开
            list = _hardWareEvents[(int)HardWareEventType.KeyUp];
            foreach (var item in list)
            {
                if (Input.GetKeyUp((KeyCode)item.Key))
                {
                    SendEvent(item.Value);
                }
            }
        }
    }


    public void MouseLoop()
    {
        if (!LockMouse)
        {
            Event mouse = Event.current;
            SortedList<int, List<HardWareEventHandler>> list;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                notClickUI = true;
            }
            else
            {
                notClickUI = false;
            }

            if (mouse.isMouse && mouse.type == EventType.MouseDown && notClickUI)
            {
                if (mouse.clickCount == 2)
                {           
                    // 双击鼠标的某个键
                    list = _hardWareEvents[(int)HardWareEventType.MouseDBClick];
                    foreach (KeyValuePair<int, List<HardWareEventHandler>> kvp in list)
                    {
                        if (mouse.button == kvp.Key)
                        {
                            SendEvent(kvp.Value);
                        }
                    }
                }
                else
                {
                    // 按下鼠标的某个键
                    list = _hardWareEvents[(int)HardWareEventType.MouseDown];
                    foreach (KeyValuePair<int, List<HardWareEventHandler>> kvp in list)
                    {
                        if (mouse.button == kvp.Key)
                        {
                            SendEvent(kvp.Value);
                        }
                    }
                }
            }
            
            // 松开鼠标的某个键
            if (mouse.isMouse && mouse.type == EventType.MouseUp)
            {
                list = _hardWareEvents[(int)HardWareEventType.MouseUp];
                foreach (KeyValuePair<int, List<HardWareEventHandler>> kvp in list)
                {
                    if (mouse.button == kvp.Key)
                    {
                        SendEvent(kvp.Value);
                    }
                }
            }

            // 拖动鼠标
            if (mouse.isMouse && mouse.type == EventType.MouseDrag && notClickUI)
            {
                list = _hardWareEvents[(int)HardWareEventType.MouseDrag];
                foreach (KeyValuePair<int, List<HardWareEventHandler>> kvp in list)
                {
                    if (mouse.button == kvp.Key)
                    {
                        SendEvent(kvp.Value);
                    }
                }
            }

            // 鼠标移动
            list = _hardWareEvents[(int)HardWareEventType.MouseMove];
            _mouseMoveX = Input.GetAxis("Mouse X");
            _mouseMoveY = Input.GetAxis("Mouse Y");
            if (_mouseMoveX != 0.0f || _mouseMoveY != 0.0f)
            {
                foreach (var item in list)
                {
                    SendEvent(item.Value);
                }
            }

            // 鼠标滚轮
            list = _hardWareEvents[(int)HardWareEventType.MouseScroll];
            _mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            if (_mouseScroll != 0.0f && notClickUI)
            {
                foreach (var item in list)
                {
                    SendEvent(item.Value);
                }
            }
        }
    }

    /// <summary>
    /// 添加输入设备的事件处理函数
    /// </summary>
    /// <param name="type">设备类型</param>
    /// <param name="code">鼠标类型：0 -> 左键，2 -> 中键，1 -> 右键; 键盘：Key</param>
    /// <param name="handler">处理函数</param>
    public void AddHandler(HardWareEventType type, int code, HardWareEventHandler handler)
    {
        SortedList<int, List<HardWareEventHandler>> list = _hardWareEvents[(int)type];
        if (!list.ContainsKey(code))
        {
            list.Add(code, new List<HardWareEventHandler>());
        }

        List<HardWareEventHandler> handlers = list[code];
        handlers.Add(handler);
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="code"></param>
    /// <param name="handler"></param>
    public void RemoveHandler(HardWareEventType type, int code, HardWareEventHandler handler)
    {
        SortedList<int, List<HardWareEventHandler>> list = _hardWareEvents[(int)type];
        if (list.ContainsKey(code))
        {
            List<HardWareEventHandler> handlers = list[code];
            handlers.Remove(handler);
            if (handlers.Count == 0)
            {
                list.Remove(code);
            }
        }
    }
    #endregion

    #region private method
    private void SendEvent(List<HardWareEventHandler> events)
    {
        for (int i = events.Count - 1; i >= 0; i--)
        {
            if (events[i] == null)
            {
                events.RemoveAt(i);
                continue;
            }
            events[i]();
        }
    }
    #endregion
}
