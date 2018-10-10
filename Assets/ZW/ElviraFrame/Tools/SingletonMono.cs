/*******
* Copyright (C)2018    Administrator 
* 创建人:              Administrator  
* 创建时间:            2018/6/14 星期四 14:09:06
 *
 *
 * 单例基类
****************************/
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/// <summary>
/// 单例基类，是Mono的单例
/// </summary>
public abstract class SingletonMono<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T m_instance = null;

    public static T Instance
    {
        get { return m_instance; }
    }

    public virtual void Awake()
    {
        m_instance = this as T;
    }
}
