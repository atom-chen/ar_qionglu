using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstUseTipManager :SingletonMono<FirstUseTipManager>
{


    private GameObject tipImage;
    public bool isTipTour = true;
    int state = 0;
    
    public override void Awake()
    {
        base.Awake();

        tipImage = transform.GetChild(0).gameObject;
        CheckHasSaw();
    }


    private void Start()
    {     
        
    }
    public void ShowNextTip(TipType  nextTipType)
    {
        if (isTipTour==false)
        {
            return;
        }
        if (state!=(int)nextTipType)
        {
            return;
        }
        switch (nextTipType)
        {
            case TipType.ChooseModelTip:
                tipImage = transform.GetChild(0).gameObject;
                break;
            case TipType.WriteTip:
                tipImage = transform.GetChild(1).gameObject;
                break;
            case TipType.EffectTip:
                tipImage = transform.GetChild(2).gameObject;
                break;
            case TipType.ShotTip:
                tipImage = transform.GetChild(3).gameObject;
                break;
            case TipType.ShareTip:
                tipImage = transform.GetChild(4).gameObject;
                break;
            default:
                break;
        }

        tipImage.gameObject.SetActive(true);

        state++;
    }
    /// <summary>
    /// 检测首次进入是否需要提示
    /// </summary>
    private void CheckHasSaw()
    {
        if (PlayerPrefs.HasKey(GlobalParameter.isHasSaw) == false)
        {
            isTipTour = true;
         
            tipImage.gameObject.SetActive(true);
            ShowNextTip(TipType.ChooseModelTip);
        }
        else
        {
            isTipTour = false;
            tipImage.gameObject.SetActive(false);
        }
    }
    public void FirtstUseTipHasSaw()
    {
        isTipTour = false;
        PlayerPrefs.SetInt(GlobalParameter.isHasSaw, 6666);
    }

    public void TipHide()
    {
        tipImage.gameObject.SetActive(false);
    }
/// <summary>
/// 选模型提示
/// </summary>
    public void Button1111Click()
    {
        TipHide();
#if UNITY_IOS || UNITY_IPHONE
         PlaneManager.Instance.SetMode("shitou");
#elif UNITY_ANDROID
        if (SceneManager.GetActiveScene().name == "yiyou")
        {
            PlaneManager.Instance.SetMode("shitou");
        }
        else
        {
            WikiSLAMController.Instance.SetMode("shitou");
        }
#endif
    }
    /// <summary>
    /// 写字提示
    /// </summary>
    public void Button2222Click()
    {
        TipHide();
        //下一步调整提示
        ShowNextTip(TipType.EffectTip);
    }
    /// <summary>
    /// 调整模型提示
    /// </summary>
    public void Button3333Click()
    {
        TipHide();

    }

    /// <summary>
    /// 拍照提醒
    /// </summary>
    public void Button4444Click()
    {
        TipHide();
    }

    /// <summary>
    /// 分享
    /// </summary>
    public void Button5555Click()
    {
        TipHide();
        FirtstUseTipHasSaw();
    
    }

}
