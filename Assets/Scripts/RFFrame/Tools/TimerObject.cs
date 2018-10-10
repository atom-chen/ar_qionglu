/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     Timer.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-26 
 *Description:    
 *History: 
*/
using UnityEngine;
using System.Collections;

public delegate void TimerTriggerCallback(TimerObject timerObj);

public class TimerObject
{
    /// <summary>  
    /// 定时器GUID;  
    /// </summary>  
    private int Guid;
    /// <summary>  
    /// 起点;  
    /// </summary>  
    private int startTick;
    /// <summary>  
    /// 终点;  
    /// </summary>  
    private int endTick;

    /// <summary>  
    /// 间隔多长时间触发;  
    /// </summary>  
    private int triggerTick;

    private bool isOver = false;

    /// <summary>  
    /// 触发回调;  
    /// </summary>  
    private TimerTriggerCallback Callback;
    //////////////////////////////////////////////////////////////////////////  


    ////////////////////////////////内部逻辑变量/////////////////////////////  
    /// <summary>  
    /// 当前时间;  
    /// </summary>  
    private float curTick;

    /// <summary>  
    /// 定时累加时间;  
    /// </summary>  
    private int delta = 0;
    private bool running = false;
    //////////////////////////////////////////////////////////////////////////  

    #region 属性访问器;  
    /// <summary>  
    /// 定时器GUID;  
    /// </summary>  
    public int TimerGuid
    {
        get
        {
            return Guid;
        }
    }
    /// <summary>  
    /// 起点;  
    /// </summary>  
    public int StartTick
    {
        get
        {
            return startTick / 1000;
        }
    }
    /// <summary>  
    /// 起点;  
    /// </summary>  
    public int EndTick
    {
        get
        {
            return endTick / 1000;
        }
    }
    /// <summary>  
    /// 当前时间;  
    /// </summary>  
    public int CurTick
    {
        get
        {
            return (int)Mathf.Round((curTick + 499.9f) / 1000.0f);
        }
    }
    /// <summary>  
    /// 间隔多长时间触发;  
    /// </summary>  
    public int TriggerTick
    {
        get
        {
            return triggerTick / 1000;
        }
    }
    /// <summary>  
    /// 是否已经结束;  
    /// </summary>  
    public bool IsOver
    {
        get
        {
            return isOver;
        }
    }
    #endregion
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="guid">定时器GUID</param>
    /// <param name="st">起点</param>
    /// <param name="et">终点</param>
    /// <param name="tt">触发间隔时间</param>
    /// <param name="callback"></param>
    public TimerObject(int guid, int st, int et, int tt, TimerTriggerCallback callback)
    {
        Guid = guid;
        startTick = st;
        endTick = et;
        curTick = st;
        triggerTick = tt;
        Callback = callback;
        delta = 0;
        isOver = false;
        running = true;
    }

    /// <summary>  
    /// 开启定时器(因为定时器生成后就会运行，这个接口主要是对应Pause);  
    /// </summary>  
    public void Play()
    {
        running = true;
    }

    /// <summary>  
    /// 暂停定时器;  
    /// </summary>  
    public void Pause()
    {
        running = false;
    }

    /// <summary>  
    /// 停止定时器;  
    /// </summary>  
    public void Stop()
    {
        curTick = endTick;
    }

    /// <summary>  
    /// 删除定时器  
    /// </summary>  
    public void Over()
    {
        isOver = true;
    }

    /// <summary>  
    /// 重置定时器，注意，掉用此函数后，要将此定时器对象重新注册到管理类里面，否则可能不会触发定时器事件;  
    /// </summary>  
    public void Reset()
    {
        curTick = startTick;
        delta = 0;
        isOver = false;
        running = true;

        Callback(this);
    }

    /// <summary>  
    /// 更新定时器;  
    /// </summary>  
    public bool UpdateTick(int tickInMillionSeconds)
    {
        if (running)
        {
            if (startTick > endTick)
            {
                curTick -= tickInMillionSeconds;
                if (curTick <= endTick)
                {
                    isOver = true;
                }
            }
            else
            {
                curTick += tickInMillionSeconds;
                if (curTick >= endTick)
                {
                    isOver = true;
                }
            }

            delta += tickInMillionSeconds;
            UnityEngine.Debug.Log("curTick" + curTick + "  delta:" + delta + "  isOver:" + isOver);
            if (delta >= triggerTick)
            {
                delta -= triggerTick;
                Callback(this);
            }
            else if (isOver)
            {
                Callback(this);
            }

            return isOver;
        }
        else
        {
            return false;
        }
    }
}  

