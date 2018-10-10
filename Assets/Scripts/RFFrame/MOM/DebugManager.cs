/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     DebugManager.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-27 
 *Description:    
 *History: 
*/
using UnityEngine;
/// <summary>
/// 输出控制
/// </summary>
public class DebugManager : Singleton<DebugManager>
{

    public bool DoDebug;

    protected override void Init()
    {
        base.Init();
        DoDebug = true;
    }
    public void Log(string message)
    {
        if (DoDebug)
        {
           //Debug.Log(message);
        }
    }
    public void LogWaring(string message)
    {
        if (DoDebug)
        {
            //Debug.LogWarning(message);
        }
    }
    public void LogError(string message)
    {
        if (DoDebug)
        {
            //Debug.LogError(message);
        }
    }
}
