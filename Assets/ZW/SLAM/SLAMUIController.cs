using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using ElviraFrame.AB;
using ElviraFrame;




/// <summary>
/// SLAM    UI    基类
/// </summary>
public class SLAMUIController : SingletonMono<SLAMUIController>
{
    public GameObject firstUseTip;
    /// <summary>
    /// 检测首次进入是否需要提示
    /// </summary>
    private void CheckHasSaw()
    {
        if (PlayerPrefs.HasKey(GlobalParameter.isHasSaw) == false)
        {
            firstUseTip.gameObject.SetActive(true);
        }
        else
        {
            firstUseTip.gameObject.SetActive(false);
        }
    }

    public override void Awake()
    {
        base.Awake();



  //      CheckHasSaw();

    }


    public void FirtstUseTipClick()
    {
        PlayerPrefs.SetInt(GlobalParameter.isHasSaw, 6666);
    }
}
