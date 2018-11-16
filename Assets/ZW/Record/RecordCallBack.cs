using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordCallBack : MonoBehaviour {

    /// <summary>
    /// 录屏开始回调
    /// </summary>
    public void onCaptureRecodeStart()
    {
        ButtonPanelUI.Instance.onCaptureRecodeStart();
    }
    /// <summary>
    /// 录屏失败回调
    /// </summary>
    /// <param name="msg"></param>
    public void onCaptureRecodeFailed(string msg)
    {
        ButtonPanelUI.Instance.onCaptureRecodeFailed(msg);
    }
    /// <summary>
    /// 录屏结束(保存之后)
    /// </summary>
    /// <param name="msg"></param>
    public void onCaptureRecodeStop()
    {
      //  ButtonPanelUI.Instance.onCaptureRecodeFailed(msg);
    }
    /// <summary>
    /// 录屏(取消保存)
    /// </summary>
    /// <param name="msg"></param>
    public void onCaptureRecodeSave()
    {
        //  ButtonPanelUI.Instance.onCaptureRecodeFailed(msg);
    }
}
