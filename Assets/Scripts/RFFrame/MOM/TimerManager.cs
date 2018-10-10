/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     TimerManager.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-26 
 *Description:    
 *History: 
*/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 时间类
/// </summary>
public class TimerManager : Singleton<TimerManager>
{
    private  float time;

    private Dictionary<object, TimerItem> timerList = new Dictionary<object, TimerItem>();

    public  void Loop()

    {
        // 设置时间值
        time = Time.time;
        TimerItem[] objectList = new TimerItem[timerList.Values.Count];
        timerList.Values.CopyTo(objectList, 0);
        // 锁定
        foreach (TimerItem timerItem in objectList)
        {
            if (timerItem != null) timerItem.Run(time);
        }
    }
    public  void Register(object objectItem, float delayTime, Action callback)
    {
        if (!timerList.ContainsKey(objectItem))
        {
            TimerItem timerItem = new TimerItem(time, delayTime, callback);
            timerList.Add(objectItem, timerItem);
        }
    }
    public  void UnRegister(object objectItem)
    {
        if (timerList.ContainsKey(objectItem))
        {
            timerList.Remove(objectItem);
        }
    }
}

public class TimerItem
{
    /// <summary>
    /// 当前时间
    /// </summary>
    public float currentTime;

    /// <summary>
    /// 延迟时间
    /// </summary>
    public float delayTime;

    /// <summary>
    /// 回调函数
    /// </summary>
    public Action callback;

    public TimerItem(float time, float delayTime, Action callback)

    {
        this.currentTime = time;
        this.delayTime = delayTime;
        this.callback = callback;
    }
    public void Run(float time)

    {
        // 计算差值
        float offsetTime = time - this.currentTime;
        // 如果差值大等于延迟时间
        if (offsetTime >= this.delayTime)
        {
            float count = offsetTime / this.delayTime - 1;
            float mod = offsetTime % this.delayTime;
            for (int index = 0; index < count; index++)
            {
                this.callback();
            }
            this.currentTime = time - mod;
        }
    }
}