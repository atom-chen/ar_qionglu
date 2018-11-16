using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangAn : SingletonMono<ChangAn>
{
    /// <summary>
    ///按钮长按录像 
    /// </summary>
     float timer = 1f;
    float lastTime = 0f;
    bool isLongPress = false;
 public   bool   isRecing=false;
    private void Update()
    {
        if (isLongPress && timer > 0 && lastTime > 0 && Time.time - lastTime > timer)
        {
            if (isRecing==false)
            {
                Debug.Log("长按触发");
                CaptureStart();
            }
       
            isLongPress = false;
            lastTime = 0;

        }

    }

    public void LongPress(bool bIsStart)
    {
        isLongPress = bIsStart;
        if (isLongPress)
        {
            lastTime = Time.time;
            Debug.Log("长按开始");
        }
        else// if (lastTime != 0)
        {
            lastTime = 0;
            Debug.Log("长按取消");
            if (isRecing==true)
            {
                CaptureStop();
            }
            else
            {
   Shot();
            }
         
            isRecing = false;
        }

    }

    public void CaptureStart()
    {
        if (isRecing==false)
        {
            isRecing = true;
            Debug.Log("录像开始");
            ButtonPanelUI.Instance.ShotCaptureClick();
        }

  
    }

    public void CaptureStop()
    {
        Debug.Log("录像结束");
    ButtonPanelUI.Instance.stopRecoding();

        isRecing = false;
    }
    public void Shot()
    {
        if (isRecing==false)
        {
            Debug.Log("拍照");
            ButtonPanelUI.Instance.ShotShotClick();
        }
  
    }


    public void Init()
    {
        isRecing = false;
        isLongPress = false;
        lastTime = 0;


    }
}
