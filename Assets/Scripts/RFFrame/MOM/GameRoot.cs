/** 
 *Copyright(C) 2015 by DefaultCompany 
 *All rights reserved. 
 *FileName:     GameRoot.cs 
 *Author:       若飞 
 *Version:      1.0 
 *UnityVersion：5.3.2f1 
 *Date:         2017-04-25 
 *Description:    
 *History: 
*/
using UnityEngine;
using System.Collections;
/// <summary>
/// 程序入口 负责初始化
/// </summary>
public class GameRoot : MonoBehaviour {


    private void OnGUI()
    {
        //检测鼠标事件
        HardWareManager.Instance.MouseLoop();
    }
    private void Update()
    {
        //检测键盘事件
        HardWareManager.Instance.KeyboardLoop();
        TimerManager.Instance.Loop();
    }
}
