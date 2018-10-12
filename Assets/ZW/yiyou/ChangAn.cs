using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangAn : MonoBehaviour
{
    /// <summary>
    ///按钮长按录像 
    /// </summary>
     float timer = 3f;
    float lastTime = 0f;
    bool isLongPress = false;

    private void Update()
    {
        if (isLongPress && timer > 0 && lastTime > 0 && Time.time - lastTime > timer)
        {
            Debug.Log("长按触发");
            CaptureStart();
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
        else if (lastTime != 0)
        {
            lastTime = 0;
            Debug.Log("长按取消");
            Shot();
        }

    }

    private void CaptureStart()
    {
        Debug.Log("录像开始");
        ButtonPanelUI.Instance.ShotCaptureClick();
    }

    private void Shot()
    {
        Debug.Log("拍照");
        ButtonPanelUI.Instance.ShotShotClick();
    }
}
